import pandas as pd
import numpy as np
import neuron as neuron

playoff_df = pd.read_csv('playoffs.csv')
playoff_norm_df = (playoff_df - playoff_df.mean()) / (playoff_df.max() - playoff_df.min())
#playoff_norm_df["HighRound"] = playoff_df["HighRound"]

#print(playoff_norm_df)

num_inputs = len(playoff_norm_df.columns) - 5
playoff_class = neuron.NeuralNetwork(num_inputs, 1, 5)

playoff_norm_df.iloc[:,0:5] = playoff_df.iloc[:,0:5]
print(playoff_norm_df.iloc[:, 6:(num_inputs + 5)])

for index, row in playoff_norm_df.iterrows():
    playoff_class.train(row[6:(num_inputs + 5)], row[0:5])


playoff_class.print_network()
