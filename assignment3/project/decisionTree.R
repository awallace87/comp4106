library(foreach)
library(R6)
library(plyr)

Entropy <- function(classProportions) {
  ls<-foreach(prop=classProportions) %do% -(prop * log2(prop))
  return(Reduce(sum, ls))
}

InformationGain <- function(parentProbs, childProbs) {
  ls<-foreach(child = childProbs) %do% (child * Entropy(child) )
  lsSum <- Reduce(sum, ls)
  return(Entropy(parentProbs) - lsSum)
}

ConstructRandomClasses <- function(numOfClasses, numOfDimensions, dependencies = FALSE) {
  startingDataFrame <- data.frame(class=paste("distinct",1:numOfClasses), matrix(runif(numOfClasses*(numOfDimensions), min = 0, max = 1)
                                         , nrow = numOfClasses, ncol = numOfDimensions), stringsAsFactors=F)
  return(startingDataFrame)
}

ConstructSamples <- function(classDefinitions, samplesPerClass) {
  numClasses <- nrow(classDefinitions)
  numDimensions <- ncol(classDefinitions) - 1
  dimNames <- colnames(classDefinitions)[2:ncol(classDefinitions)]
  
  numOfSamples <- samplesPerClass * numClasses
  sampleDF <- setNames(data.frame(matrix(ncol = numDimensions + 1, nrow = numOfSamples), stringsAsFactor=F), union("class", dimNames))
  
  for(i in 1:numOfSamples){
    sampleDF$class[i] <- i%%numClasses + 1    
  }
  #colnames(sampleDF) <- c(1:numDimensions+1)
  #colnames(sampleDF)[1] <- "class"
  
  for(i in dimNames) {
    sampleDF[[i]] <- apply(sampleDF, 1, function(classNum) as.numeric(classDefinitions[classNum[1], i] < runif(1, 0.0, 1.0)) )
  }
  
  return(sampleDF)
}

DTNode <- R6Class("DTNode",
                  public = list(
                    initialize = function(training.features, training.labels) {
                      #Determine Entropy of Labels
                      count.labels <- as.data.frame(table(training.labels), stringsAsFactor=F)
                      label.probs <- count.labels$Freq / length(training.labels)
                      label.entropy <- Entropy(label.probs)
                      if(label.entropy < 0.5) {
                        self$result.class <- count.labels$training.labels[which.max(count.labels$Freq)]
                      } else {
                        max.info.attribute <- NULL
                        for(i in colnames(training.features)) {
                          indices.with.attribute <- which(training.features[i] == TRUE)
                          labels.with.attribute <- training.labels[indices.with.attribute]
                          props.with.attribute <- as.data.frame(table(labels.with.attribute), stringsAsFactors=F)$Freq / length(labels.with.attribute)
                          weight <- length(indices.with.attribute)/length(training.labels)
                          weighted.entropy <- Entropy(props.with.attribute)
                          
                          indices.without.attribute <- which(training.features[i] == FALSE)
                          labels.without.attribute <- training.labels[indices.without.attribute]
                          props.without.attribute <- as.data.frame(table(labels.without.attribute), stringsAsFactors=F)$Freq / length(labels.without.attribute)
                          weight.without <- length(indices.with.attribute)/length(training.labels)
                          weighted.without.entropy <- Entropy(props.without.attribute)
                          
                          sum.weighted.child.entropy <- (weight*weighted.entropy + weight.without*weighted.without.entropy)
                          
                          information.gain <- label.entropy - sum.weighted.child.entropy
                          if(isTRUE(information.gain >= private$max.info.gain)) {
                            private$max.info.gain <- information.gain
                            max.info.attribute <- i
                          }
                        }
                        self$partition.attribute <- max.info.attribute
                        if(!is.null(training.features[self$partition.attribute] == TRUE)) {
                          left.training.indices <- which(training.features[self$partition.attribute] == TRUE)
                          left.training.features <- training.features[left.training.indices,]
                          left.training.labels <- training.labels[left.training.indices]
                          self$left.node <- DTNode$new(left.training.features, left.training.labels)
                        }
                        #label.left.id <- private$BuildTree(left.training.features, left.training.labels)
                        if(!is.null(training.features[self$partition.attribute] == FALSE)) {
                          right.training.indices <- which(training.features[self$partition.attribute] == FALSE)
                          right.training.features <- training.features[right.training.indices,]
                          right.training.labels <- training.labels[right.training.indices]
                          self$right.node <- DTNode$new(right.training.features, right.training.labels)
                        }
                        #label.right.id <- private$BuildTree(right.training.features, right.training.labels)
                      }                      
                    },
                    partition.attribute = NULL,
                    result.class = NULL,
                    left.node = NULL,
                    right.node = NULL
                    ),
                  private = list(
                    max.info.gain = -100
                    )
                  )

DecisionTree <- R6Class("Decision Tree",
                        public = list(
                          Initialize = function () {
                          },
                          Train = function (training.features, training.labels) {
                            #private$nodeData <- data.frame(id=integer(0), partition.attribute=character(0), result.class=character(0), left.id=integer(0), right.id=integer(0))
                            #private$BuildTree(training.features, training.labels)
                            root.node <- DTNode$new(training.features, training.labels)
                          },
                          root.node = NULL,
                          num.of.recur = 0
                          ),
                        private = list(
                          currentNodeID = 1,
                          nodeData = NULL,
                          BuildTree = function(training.features, training.labels) {
                            self$num.of.recur <- self$num.of.recur
                            node.id <- private$currentNodeID
                            private$currentNodeID <- private$currentNodeID + 1
                            #Determine Entropy of Labels
                            count.labels <- as.data.frame(table(training.labels), stringsAsFactor=F)
                            label.probs <- count.labels$Freq / length(training.labels)
                            label.entropy <- Entropy(label.probs)
                            label.partition.value <- "NULL"
                            label.result <- "NULL"
                            label.left.id <- -1
                            label.right.id <- -1
                            print("entropy - ", str(label.entropy))
                            if(label.entropy < 0.5) {
                              label.result <- count.labels$training.labels[1]
                            } else {
                              max.infogain.class <- "NULL"
                              max.infogain.value <- as.numeric(-10000)
                              for(i in colnames(training.features)) {
                                indices.with.attribute <- which(training.features[i] == TRUE)
                                labels.with.attribute <- training.labels[indices.with.attribute]
                                props.with.attribute <- as.data.frame(table(labels.with.attribute), stringsAsFactors=F)$Freq / length(labels.with.attribute)
                                weight <- length(indices.with.attribute)/length(training.labels)
                                weighted.entropy <- Entropy(props.with.attribute)
                                
                                indices.without.attribute <- which(training.features[i] == FALSE)
                                labels.without.attribute <- training.labels[indices.without.attribute]
                                props.without.attribute <- as.data.frame(table(labels.without.attribute), stringsAsFactors=F)$Freq / length(labels.without.attribute)
                                weight.without <- length(indices.with.attribute)/length(training.labels)
                                weighted.without.entropy <- Entropy(props.without.attribute)
                                
                                sum.weighted.child.entropy <- (weight*weighted.entropy + weight.without*weighted.without.entropy)
                                
                                information.gain <- as.numeric(label.entropy - sum.weighted.child.entropy)
                                if(!is.null(information.gain) & !is.null(max.infogain.value)) {
                                  if(as.numeric(information.gain) > as.numeric(max.infogain.value)) {
                                    max.infogain.class <- i
                                    max.infogain.value <- information.gain
                                  }
                                }
                              }
                              label.partition.value <- max.infogain.class
                              left.training.indices <- which(training.features[max.infogain.class] == TRUE)
                              left.training.features <- training.features[left.training.indices,]
                              left.training.labels <- training.labels[left.training.indices]
                              label.left.id <- private$BuildTree(left.training.features, left.training.labels)
                              right.training.indices <- which(training.features[max.infogain.class] == FALSE)
                              right.training.features <- training.features[right.training.indices,]
                              right.training.labels <- training.labels[right.training.indices]
                              label.right.id <- private$BuildTree(right.training.features, right.training.labels)
                            }
                            node.data <- c(node.id, label.partition.value, label.result, label.left.id, label.right.id)
                            private$nodeData <- rbind(private$nodeData, node.data)
                            return(node.id)
                          }                            
                          )) 
  
child <- runif(10, 0.0, 1.0)
parent <- runif(10, 0.0, 1.0)

d <- InformationGain(parent, child)

random.classes <- ConstructRandomClasses(4,10)
random.samples <- ConstructSamples(random.classes, 100)

decision.tree <- DecisionTree$new()
dt <- decision.tree$Train(random.samples[2:11], random.samples$class)
#class.list <- random.samples$class
#count.classes <- count(random.samples, "class")
#indices.with.prop <- which(random.samples[3] == TRUE)
#samples.with.prop <- random.samples[indices.with.prop, ]
#indices.without.prop <- which(random.samples[3] == FALSE)
#labels.without.prop <- random.samples$class[indices.without.prop]
#props.with.attribute <- as.data.frame(table(labels.without.prop), stringsAsFactors=F)$Freq / length(labels.without.prop)
