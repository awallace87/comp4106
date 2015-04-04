library(R6)

BayesClass <- R6Class("BayesClass",
                          public = list(
                            initialize = function() {
                            },
                            trainAndTest = function(trainingSet, dependency = FALSE, cvFolds = 1) {
                              numOfSamples <- nrow(trainingSet)
                              numPerFold <- numOfSamples / cvFolds
                              #Create Classifier - Assume that classes are identified in table
                              sumError <- 0
                              for(i in c(1:cvFolds)){
                                startTest <- (i - 1) * numPerFold
                                endTest <- i * numPerFold
                                testRange <- c(startTest:endTest)
                                trainRange <- setdiff(c(1:numOfSamples), testRange)
                                testDF <- trainingSet[testRange,]
                                trainDF <- trainingSet[trainRange,]
                                #train current classProbs
                                private$train(trainDF, dependency)
                                sumError <- sumError + private$test(testDF)
                              }
                              
                              return(sumError/cvFolds)
                            }
                            ),
                          private = list(
                            classProbs = NA,
                            classApriori = NA,
                            train = function(trainingSet, dependency = FALSE) {
                              numOfDimensions = ncol(trainingSet) - 1
                              class.index <- grep("class", colnames(trainingSet))
                              private$classProbs <- aggregate(trainingSet[,-class.index], by = list(class = trainingSet$class), FUN = mean)                              
                              return(private$classProbs)                                                            
                            },
                            test = function(testingSet) {
                              
                              classVec <- c(1:nrow(private$classProbs))
                              numDimensions <- ncol(testingSet) - 1
                              numMatches <- 0
                              numTests <- nrow(testingSet)
                              confusionMat <- matrix(0, nrow = nrow(private$classProbs), ncol = nrow(private$classProbs))
                              
                              for(i in c(1:numTests)) {
                                classVec <- rep(1, nrow(private$classProbs))
                                for(j in c(1:numDimensions) ) {
                                  for(k in c(1:length(classVec))) {
                                    originalVal <- classVec[k]
                                    if(testingSet[i,j+1]) {
                                      classVec[k] <- originalVal * private$classProbs[k,j+1]  
                                    } else {
                                      classVec[k] <- originalVal * (1 - private$classProbs[k,j+1])
                                    }
                                  }
                                }
                                predClass <- which.max(classVec)
                                confusionMat[predClass, testingSet$class[i]] <- confusionMat[predClass, testingSet$class[i]] + 1
                              }
                              numMatches <- 0
                              for(i in c(1:ncol(confusionMat))) {
                                numMatches <- numMatches + confusionMat[i,i]
                              }
                              #print(cat("Error - " , 1 - numMatches/numTests))
                              return(1 - numMatches/numTests)
                            }
                            )
                          )

ClassDef <- R6Class("ClassDef",
                    public = list(
                      initialize = function(numOfDimensions) {
                        private$dimensionProb = c(1:numOfDimensions)
                        private$probDepend = c(1:numOfDimensions)
                      },
                      dimensionProb = NA,
                      conditional = NA
                      )
                    )

createConditionalProbabilities <- function(numClasses, numDimensions) {
  classList <- c(1:numClasses)
  conditional <- setNames(data.frame(matrix(ncol = numDimensions + 1, nrow = numClasses)), union("class", paste("x", c(1:numDimensions))))
  conditional$class <- c(1:numClasses)
  for(i in c(1:nrow(conditional))) {
    for(j in c(2:ncol(conditional))) {
      conditional[i,j] <- j - 1
    }
  }
  
  for(h in c(1:numClasses)) {
    rootIndex <- sample(1:numDimensions, 1)
    for(i in c(2:ncol(conditional))) {
      if(i != rootIndex) {
        foundConditional <- FALSE
        condIndex <- i
        while(!foundConditional) {
          condIndex <- sample(1:numDimensions, 1)
          if(condIndex != conditional[h,i] & conditional[h, condIndex] != i) {
            foundConditional <- TRUE
          }
        }
        conditional[h, i] <- condIndex
      }
    }
  }
  return(conditional)
}

constructRandomClasses <- function(numOfClasses, numOfDimensions, dependencies = FALSE) {
  startingMat <- matrix(runif(numOfClasses*(numOfDimensions), min = 0, max = 1), nrow = numOfClasses, ncol = numOfDimensions)
  return(startingMat)
}

constructSamples <- function(classDefinitions, samplesPerClass) {
  numClasses <- nrow(classDefinitions)
  numDimensions <- ncol(classDefinitions)
  
  numOfSamples <- samplesPerClass * numClasses
  sampleDF <- setNames(data.frame(matrix(ncol = numDimensions + 1, nrow = numOfSamples)), union("class", paste("x", c(1:numDimensions))))

  for(i in 1:numOfSamples){
    sampleDF$class[i] <- i%%numClasses + 1
    
  }
  #colnames(sampleDF) <- c(1:numDimensions+1)
  #colnames(sampleDF)[1] <- "class"
  
  for(i in 1:numDimensions) {
    sampleDF[[i+1]] <- apply(sampleDF, 1, function(classNum) as.numeric(classDefinitions[classNum[1], i] < runif(1, 0.0, 1.0)) )
  }
  
  return(sampleDF)
}

formatData <- function(original.data, column.means) {
  class.index <- grep("class", colnames(original.data))
  for(i in 1:length(column.means)) {
    col.num <- i
    if(i >= class.index) {
      col.num <- i+1
    }
    for(j in 1:nrow(original.data)) {
      original.data[j,col.num] <- (original.data[j,col.num] > column.means[col.num])
    }
  }
  return(original.data)
}




  

classes <- 4
dimensions <- 10

num.runs <- 10
sum.error <- 1
# for(i in 1:num.runs) {
#   a <- constructRandomClasses(classes, dimensions)
#   b <- constructSamples(a, 2000)
#   
#   bayesClass <- BayesClass$new()
#   
#   artificial <- bayesClass$trainAndTest(b, FALSE, 8)
#   sum.error <- artificial + sum.error
# }
# 
# average.error <- sum.error / num.runs

# iris.data <- read.csv("iris.csv")
# iris.num.columns <- ncol(iris.data)
# colnames(iris.data)[iris.num.columns] <- "class"
# iris.column.means <- colMeans(iris.data[,-iris.num.columns])
# iris.formatted <- formatData(iris.data, iris.column.means)
# iris.acc <- bayesClass$trainAndTest(iris.formatted, FALSE, 4)

heart.disease.data <- read.csv("heartDisease.csv")
colnames(heart.disease.data)[ncol(heart.disease.data)] <- "class"
heart.disease.column.means <- colMeans(heart.disease.data[,-ncol(heart.disease.data)])
heart.disease.formatted <- formatData(heart.disease.data, heart.disease.column.means)
heart.disease.acc <- bayesClass$trainAndTest(heart.disease.formatted, FALSE, 4)

# wine.data <- read.csv("wine.csv")
# colnames(wine.data)[1] <- "class"
# wine.data <- data.frame(wine.data[,2:ncol(wine.data)], class=wine.data$class)
# wine.column.means <- colMeans(wine.data[,-ncol(wine.data)])
# wine.formatted <- formatData(wine.data, wine.column.means)
# wine.acc <- bayesClass$trainAndTest(wine.formatted, FALSE, 4)
#d <- createConditionalProbabilities(4, 10)