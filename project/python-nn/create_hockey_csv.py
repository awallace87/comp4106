import pandas as pd
import math


MAIN_PLAYER_SEASON_STATS_FILENAME = "main_player_season_csv"

hockeyref_filename = "HockeyReference_skater_{0}_season.csv"

current_year = 2015
lowest_year = 2011

def build_player_csv_hockeyref():
    season_stats_collection = dict()
    for year in range(lowest_year, current_year):
        year_file_name = hockeyref_filename.format(year)
        year_data_frame = pd.read_csv(year_file_name)
        year_data_frame["Year"] = year
        season_stats_collection[str(year)] = year_data_frame

    season_stats_df = pd.concat(season_stats_collection)
    season_stats_df = clean_hockeyref_data(season_stats_df)
    season_stats_df.to_csv("hockeyref_combine_player_season.csv")

    return season_stats_df

def get_seconds_from_time(minute_second_string):
    time_vars = minute_second_string.split(":")
    return(int(time_vars[0]) * 60 + int(time_vars[1]))

def clean_hockeyref_data(hockeyref_df):
    hockeyref_df["ATOI"] = hockeyref_df["ATOI"].apply(get_seconds_from_time)
    hockeyref_df["Player"] = hockeyref_df["Player"].apply(lambda x: str(x).upper())
    hockeyref_df = hockeyref_df.drop(hockeyref_df.columns[0], axis = 1)
    #shockeyref_df = hockeyref_df.drop(hockeyref_df.columns[0], axis = 1)
    return hockeyref_df

def calculate_fantasy_score(df_row):#, goals_ind, assists_ind, shots_ind, shgoal_ind, shassist_ind):
    goals = df_row["G"] * 3
    assists = df_row["A"] * 2
    shots_on_goal = df_row["S"]
    short_hand_points = df_row["SHG"] + df_row["SHA"]
    return(goals + assists + shots_on_goal + short_hand_points)

def fantasy_score(goals, assists, shots, sh_goals, sh_assists):
    return(goals * 3 + assists * 2 + shots + sh_goals + sh_assists)

def add_next_season_fantasy(original_df):
    original_df["Fantasy"] = original_df.apply(lambda row: calculate_fantasy_score(row), axis = 1)
    #for index, row in original_df.iterrows():
    #    goals_ind = original_df.columns.get_loc("G")
    #    assists_ind = original_df.columns.get_loc("A")
    #    shots_ind = original_df.columns.get_loc("S")
    #    sh_goal_ind = original_df.columns.get_loc("SHG")
    #    sh_assist_ind = original_df.columns.get_loc("SHA")
    #    row["Fantasy"] = calculate_fantasy_score(row, goals_ind, assists_ind, shots_ind, sh_goal_ind, sh_assist_ind)
    #Sort out players that were healthy scratches or not very active
    original_df = original_df[original_df.Fantasy > 10]
    #original_df.drop(original_df.columns[2], axis = 1, inplace = True)
    original_df.to_csv("hockeyref_combine_player_fantasy.csv")
    return original_df

hockeyanalysis_filename = "HockeyAnalysis_skater_{0}_season.csv"

def convert_name(name):
    parts = name.split(", ")
    return(parts[1] + " " + parts[0])

def add_hockeyanalysis_data(original_df):
    season_stats_collection = dict()
    for year in range(lowest_year, current_year):
        year_file_name = hockeyanalysis_filename.format(year)
        year_data_frame = pd.read_csv(year_file_name)
        year_data_frame["Year"] = year
        season_stats_collection[str(year)] = year_data_frame

    season_stats_df = pd.concat(season_stats_collection)
    #season_stats_df = clean_hockeyanalysis_data(season_stats_df)
    season_stats_df.to_csv("hockeyanalysis_season.csv")


def clean_hockeyanalysis_data(original_df):
    original_df["Player Name"] = original_df["Player Name"].apply(lambda x: convert_name(str(x)))
    return(original_df)


if __name__ == "__main__":
    original_df = build_player_csv_hockeyref()
    original_df = add_next_season_fantasy(original_df)
    add_hockeyanalysis_data(original_df)
