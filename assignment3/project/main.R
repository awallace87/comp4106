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
                              private$classProbs <- aggregate(trainingSet[,c(2:ncol(trainingSet))], by = list(class = trainingSet$class), FUN = mean)                              
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


constructRandomClasses <- function(numOfClasses, numOfDimensions) {
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

createDependenceTree <- function() {
  
}


  

classes <- 4
dimensions <- 10

a <- constructRandomClasses(classes, dimensions)
b <- constructSamples(a, 2000)

bayesClass <- BayesClass$new()
dataCols <- dimensions + 1

c <- bayesClass$trainAndTest(b, FALSE, 4)
