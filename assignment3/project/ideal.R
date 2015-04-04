library(rpart)
require(tree)

iris.data <- read.csv("iris.csv")
colnames(iris.data) <- c("sepal.length", "sepal.width", "petal.length", "petal.width", "species")

iris.tree <- tree(species ~ sepal.length + sepal.width + petal.length + petal.width, data = iris.data )


iris.ctree <- ctree(species ~ sepal.length + sepal.width + petal.length + petal.width, data = iris.data )
summary(iris.ctree)
iris.pred <- predict(iris.ctree, iris.data)
summary(iris.tree)


heart.disease.data <- read.csv("heartDisease.csv")
colnames(heart.disease.data) <- c("age", "gender", "cp", "trest.bps", "chol", "fbs"
                                  , "reste.cg", "thalach", "exang", "oldpeak", "dlope"
                                  , "ca", "thal", "patient.class")

heart.disease.data$patient.class <- as.factor(heart.disease.data$patient.class)

heart.disease.tree <- tree( patient.class ~ age + gender + cp + trest.bps + chol 
                              + fbs + reste.cg + thalach + exang + oldpeak + dlope 
                              + ca + thal, data = heart.disease.data)

summary(heart.disease.tree)
plot(heart.disease.tree)
text(heart.disease.tree, all=T)

wine.data <- read.csv("wine.csv")
colnames(wine.data) <- c("wine.class", "alcohol", "malic.acid", "ash", "alkalinity.ash", "magnesium"
                         , "total.phenols", "flavanoids", "nonflav.phenols", "proanthocyanins", "color.intensity"
                         , "hue", "od280.od315", "proline" )
wine.data$wine.class <- as.factor(wine.data$wine.class)

wine.tree <- tree( wine.class ~ alcohol + malic.acid + ash + alkalinity.ash + magnesium
                    + total.phenols + flavanoids + nonflav.phenols + proanthocyanins
                    + color.intensity + hue + od280.od315 + proline, data=wine.data )

summary(wine.tree)
plot(wine.tree)
text(wine.tree, all=T)

constructRandomClasses <- function(numOfClasses, numOfDimensions, dependencies = FALSE) {
  startingMat <- matrix(runif(numOfClasses*(numOfDimensions), min = 0, max = 1), nrow = numOfClasses, ncol = numOfDimensions)
  return(startingMat)
}

constructSamples <- function(classDefinitions, samplesPerClass) {
  numClasses <- nrow(classDefinitions)
  numDimensions <- ncol(classDefinitions)
  
  numOfSamples <- samplesPerClass * numClasses
  sampleDF <- data.frame(matrix(ncol = numDimensions + 1, nrow = numOfSamples))
  colnames(sampleDF)[1] <- "class"
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

a <- constructRandomClasses(classes, dimensions)
random.data <- constructSamples(a, 1800)
random.data$class <- as.factor(random.data$class)

random.tree <- tree(class ~ X2 + X3 + X4 + X5 + X6 + X7 + X8 + X9 + X10 + X11, data=random.data)
summary(random.tree)
plot(random.tree)
text(random.tree, all=T)

