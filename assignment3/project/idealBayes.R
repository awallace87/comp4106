library(e1071)
library(class)

iris.data <- read.csv("iris.csv")
colnames(iris.data) <- c("sepal.length", "sepal.width", "petal.length", "petal.width", "species")

iris.class <- naiveBayes(iris.data[1:4], iris.data[,5])
iris.predict <- table(predict(iris.class, iris.data[,-5]), iris.data[,5])

iris.correct.sum <- 0
for(i in 1:ncol(iris.predict)) {
  iris.correct.sum <- iris.predict[i,i] + iris.correct.sum
}
iris.accuracy <- 1 - iris.correct.sum / nrow(iris.data)
print(iris.predict)

heart.disease.data <- read.csv("heartDisease.csv")
colnames(heart.disease.data) <- c("age", "gender", "cp", "trest.bps", "chol", "fbs"
                                  , "reste.cg", "thalach", "exang", "oldpeak", "dlope"
                                  , "ca", "thal", "patient.class")

heart.disease.data$patient.class <- as.factor(heart.disease.data$patient.class)

patient.class.ind <- grep("patient.class", colnames(heart.disease.data))

heart.disease.classifier <- naiveBayes(heart.disease.data[1:13], heart.disease.data[,patient.class.ind])
heart.disease.predict <- table( predict(heart.disease.classifier, heart.disease.data[,-patient.class.ind])
                                , heart.disease.data[,patient.class.ind])
print(heart.disease.predict)
hd.correct.sum <- 0
for(i in 1:ncol(heart.disease.predict)) {
  hd.correct.sum <- heart.disease.predict[i,i] + hd.correct.sum
}
hd.accuracy <- 1 - hd.correct.sum / nrow(heart.disease.data)

wine.data <- read.csv("wine.csv")
colnames(wine.data) <- c("wine.class", "alcohol", "malic.acid", "ash", "alkalinity.ash", "magnesium"
                         , "total.phenols", "flavanoids", "nonflav.phenols", "proanthocyanins", "color.intensity"
                         , "hue", "od280.od315", "proline" )
wine.data$wine.class <- as.factor(wine.data$wine.class)

wine.class.ind <- grep("wine.class", colnames(wine.data))
wine.classifier <- naiveBayes(wine.data[2:13], wine.data[,wine.class.ind])
wine.predict <- table(predict(wine.classifier, wine.data[,-wine.class.ind])
                      , wine.data[,wine.class.ind])
print(wine.predict)
wine.correct.sum <- 0
for(i in 1:ncol(wine.predict)) {
  wine.correct.sum <- wine.predict[i,i] + wine.correct.sum
}
wine.accuracy <- 1 - wine.correct.sum / nrow(wine.data)

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

classes <- 4
dimensions <- 10

num.runs <- 10
sum.error <- 0
# for(i in 1:num.runs) {
#   a <- constructRandomClasses(classes, dimensions)
#   random.data <- constructSamples(a, 1800)
#   random.data$class <- as.factor(random.data$class)
#   random.test <- constructSamples(a, 200)
#   random.test$class <- as.factor(random.test$class)
#   
#   random.class.ind <- grep("class", colnames(random.data))
#   
#   random.classifier <- naiveBayes(random.data[2:11], random.data[,random.class.ind])
#   random.predict <- table(predict(random.classifier, random.test[,-random.class.ind])
#                             , random.test[,random.class.ind])
#   correct.sum <- 0
#   for(i in 1:ncol(random.predict)) {
#     correct.sum <- random.predict[i,i] + correct.sum
#   }
#   sum.error <- sum.error + (1 - correct.sum / nrow(random.test))
# }
# 
# average.rand.error <- sum.error / num.runs

