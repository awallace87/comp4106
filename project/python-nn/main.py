import pandas as pd
import numpy as np
import neuron as neuron
from enum import Enum
import random

class PlayoffPosition(Enum):
    Champ = 0
    Finalist = 1
    Conference = 2
    Semi = 3
    Quarter = 4

#Playoff Prediction
PLAYOFF_EPOCH = 8
def make_playoff_position_preds():
    playoff_df = pd.read_csv('playoffs.csv')
    playoff_norm_df = (playoff_df - playoff_df.mean()) / (playoff_df.max() - playoff_df.min())

    num_inputs = len(playoff_norm_df.columns) - 5
    playoff_class = neuron.NeuralNetwork(num_inputs, 3, 5)

    playoff_norm_df.iloc[:,:5] = playoff_df.iloc[:,:5]

    #Add in multiple epoch training
    epoch_num = 0
    index_sequence = range(len(playoff_norm_df))
    index_len = len(index_sequence)
    split_ind = int(index_len * 0.8)
    total_tests = 0.0
    total_success = 0.0
    while epoch_num < PLAYOFF_EPOCH:
        random.shuffle(index_sequence)
        train_list = index_sequence[:split_ind]
        test_list = index_sequence[(-split_ind):]
        total_tests += len(test_list)
        for i in train_list:
            row = playoff_norm_df.iloc[i]
            playoff_class.train(row[5:], row[:5])
        for j in test_list:
            row = playoff_norm_df.iloc[j]
            results = playoff_class.test(row[5:])
            actual = list(row[:5])
            if(results.index(max(results)) == actual.index(max(actual))):
                total_success += 1
                #print("Success")
        epoch_num += 1
        print("Finished Epoch")

    #cross-validation
    success_rate = float(total_success / total_tests)
    print("Overall Rate of Success: {0}".format(total_success / total_tests))

    #predict for latest season
    current_season_df = pd.read_csv("currentseason.csv")
    norm_cols = list(current_season_df.columns[1:])

    #print("Current Season Cols - {0}".format(len(current_season_df.columns[1:])))
    #print("Regular Cols - {0}".format(len(playoff_norm_df.columns[5:])))


    for col in norm_cols:
        current_season_df[col] = (current_season_df[col] - current_season_df[col].mean()) / (current_season_df[col].max() - current_season_df[col].min())

    #Only TOI 3v3 has a NaN entry, and in other tables it is consistently -0.015625
    current_season_df = current_season_df.fillna(-0.015625)

    #print(current_season_df)

    #TODO Create Table of Playoff Percentages


    for index, row in current_season_df.iterrows():
        result = playoff_class.test(row[1:])
        print("Team - {0}".format(row[0]))
        for i in range(len(result)):
            print("Round - {0}, Prob - {1}".format(PlayoffPosition(i), result[i]))
        #enum_pos = PlayoffPosition(result.index(max(result)))
        #print(row[0] + " - Predicted Playoff Position = {0}, with prob - {1}".format(enum_pos, max(result)))

PLAYER_FANTASY_EPOCH = 1

def make_fantasy_player_preds():

    hockeyref_fantasy_file = "hockeyref_combine_player_fantasy.csv"
    player_fantasy_df = pd.read_csv(hockeyref_fantasy_file)
    player_fantasy_df.fillna(value=0.0)

    num_inputs = len(player_fantasy_df.columns)

    norm_fantasy_cols = list(player_fantasy_df.columns[6:].values)
    norm_fantasy_cols.remove("Year")
    norm_fantasy_cols.append("Age")

    num_train_inputs = len(norm_fantasy_cols)

    player_norm_df = player_fantasy_df
    for col in norm_fantasy_cols:
        player_norm_df[col] = (player_fantasy_df[col] - player_fantasy_df[col].mean()) / (player_fantasy_df[col].max() - player_fantasy_df[col].min())

    player_epoch_num = 0
    index_sequence = range(len(player_norm_df))

    fantasy_class = neuron.NeuralNetwork(num_train_inputs, 5, 1)

    index_len = len(index_sequence)
    split_ind = int(index_len * 0.8)
    total_tests = 0.0
    total_success = 0
    b_success = 0
    c_success = 0

    while player_epoch_num < PLAYER_FANTASY_EPOCH:
        random.shuffle(index_sequence)
        for i in index_sequence[:split_ind]:
            row = player_norm_df.iloc[i]
            row_name = row["Player"]
            row_year = row["Year"]
            next_year_df = player_norm_df[(player_norm_df["Player"] == row_name) | (player_norm_df["Year"] == (row_year + 1))]
            if len(next_year_df) > 0:
                #print("Found Next Year")
                first_entry = next_year_df.iloc[0]
                #print(first_entry["Fantasy"])
                fantasy_class.train(row[6:], [(first_entry["Fantasy"])])
        for j in index_sequence[split_ind:]:
            row = player_norm_df.iloc[j]
            row_name = row["Player"]
            row_year = row["Year"]
            next_year_df = player_norm_df[(player_norm_df["Player"] == row_name) | (player_norm_df["Year"] == (row_year + 1))]
            if len(next_year_df) > 0:
                total_tests += 1
                first_entry = next_year_df.iloc[0]
                result_fantasy = fantasy_class.test(row[6:])
                diff_fantasy = float(result_fantasy[0]) - float(first_entry["Fantasy"])
                #print("Fantasy Test, Expected Val = {0}, Result = {1}".format(float(first_entry["Fantasy"]), result_fantasy))
                if(diff_fantasy < 0.025 and diff_fantasy > -0.025):
                    total_success += 1
                if(diff_fantasy < 0.05 and diff_fantasy > -0.05):
                    b_success += 1
                if(diff_fantasy < 0.1 and diff_fantasy > -0.1):
                    c_success += 1
        print("Epoch Complete")
        player_epoch_num += 1

    print("A Predictions Success Rate = {0}".format(total_success / total_tests))
    print("B Predictions Success Rate = {0}".format(b_success / total_tests))
    print("C Predictions Success Rate = {0}".format(c_success / total_tests))



if __name__ == "__main__":
    #make_playoff_position_preds()
    make_fantasy_player_preds()
