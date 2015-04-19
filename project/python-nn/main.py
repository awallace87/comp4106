import pandas as pd
import numpy as np
import neuron as neuron
import random


#Playoff Prediction
PLAYOFF_EPOCH = 5

playoff_df = pd.read_csv('playoffs.csv')
playoff_norm_df = (playoff_df - playoff_df.mean()) / (playoff_df.max() - playoff_df.min())

num_inputs = len(playoff_norm_df.columns) - 5
playoff_class = neuron.NeuralNetwork(num_inputs, 1, 5)

playoff_norm_df.iloc[:,0:5] = playoff_df.iloc[:,0:5]
print(playoff_norm_df.iloc[:, 6:(num_inputs + 5)])


#Add in multiple epoch training
epoch_num = 0
index_sequence = range(len(playoff_norm_df))
while epoch_num < PLAYOFF_EPOCH:
    random.shuffle(index_sequence)
    for i in index_sequence:
        row = playoff_norm_df.iloc[i]
        playoff_class.train(row[6:(num_inputs + 5)], row[0:5])
    epoch_num += 1
    print("Finished Epoch")

#cross-validation

#predict for latest season


#NEW PREDICTIONS TO MODEL
VALID_SEASON_YEARS = ["2015","2014","2013","2011"]
hockeyref_filename = "HockeyReference_skater_{0}_season"

#gather dataframes
master_data_frame_collection = []

for year in VALID_SEASON_YEARS:
    print(hockey_ref_filename.format(year))
