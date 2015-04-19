library(neuralnet)

library(rpart)
require(tree)

playoff.data <- read.csv("playoffs.csv")

neuralnet(HighRound ~ GP + TOI.5v5 + TOI.5v4 + TOI.5v3 + 
          + TOI.4v5 + TOI.4v4 + TOI.4v3 + TOI.3v5 + TOI.3v4	+ 
            TOI.3v3	+ X5v5.TOI.Tot + X5v5.GF + X5v5.GA +
            X5v5.GF.60 + X5v5.SF.60 + X5v5.Sh. + X5v5.GA.60 + 
            X5v5.SA.60 + X5v5.Sv. + X5v5...per60 + X5v4.TOI.Tot + 
            X5v4.GF + X5v4.GA + X5v4.GF.60 + X5v4.SF.60 + 
            X5v4.Sh. + X5v4.GA.60	+ X5v4.SA.60 + X5v4.Sv. + 
            X5v4...per60 + X4v5.TOI.Tot + X4v5.GF + X4v5.GA + X4v5.GF.60 + 
            X4v5.SF.60 + X4v5.Sh. + X4v5.GA.60 + X4v5.SA.60 + X4v5.Sv. + X4v5...per60,
          , data = playoff.data, hidden = 5, rep = 4)

playoff.tree <- tree(HighRound ~ GP + TOI.5v5 + TOI.5v4 + TOI.5v3 + 
               + TOI.4v5 + TOI.4v4 + TOI.4v3 + TOI.3v5 + TOI.3v4  + 
               TOI.3v3	+ X5v5.TOI.Tot + X5v5.GF + X5v5.GA +
               X5v5.GF.60 + X5v5.SF.60 + X5v5.Sh. + X5v5.GA.60 + 
               X5v5.SA.60 + X5v5.Sv. + X5v5...per60 + X5v4.TOI.Tot + 
               X5v4.GF + X5v4.GA + X5v4.GF.60 + X5v4.SF.60 + 
               X5v4.Sh. + X5v4.GA.60	+ X5v4.SA.60 + X5v4.Sv. + 
               X5v4...per60 + X4v5.TOI.Tot + X4v5.GF + X4v5.GA + X4v5.GF.60 + 
               X4v5.SF.60 + X4v5.Sh. + X4v5.GA.60 + X4v5.SA.60 + X4v5.Sv. + X4v5...per60,
             , data = playoff.data)

summary(playoff.tree)