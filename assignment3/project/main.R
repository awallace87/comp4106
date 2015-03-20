
constructRandomClasses <- function(numOfClasses, numOfDimensions) {
  startingMat <- matrix(runif(numOfClasses*numOfDimensions, min = 0, max = 1), nrow = numOfClasses, ncol = numOfDimensions)
  return(startingMat)
}

classes <- 4
dimensions <- 10

a <- constructRandomClasses(classes, dimensions)

