# -*- coding: utf-8 -*-
"""
Created on Tue Apr  3 07:50:42 2018

@author: RoelofSchoeman
"""
import math
import numpy as np


NP = [3,3] #[Number of Ingredients, Number of packages of each ingredient]
R = [70,80,90] # [grams of each ingredient to form a single serving]
Packages = [[1260,1500,700],[800,1440,1600],[1700,1620,900]] #[jth packet volume per ingredient i - Packages[i][j]]

def PacketServingRange(PacketMass,ServingMass):
    minServings = math.ceil(PacketMass/ServingMass/1.1)
    maxServings = math.floor(PacketMass/ServingMass/0.9)
    if minServings > maxServings:
        return [0]
    else:
        return np.linspace(minServings,maxServings,maxServings-minServings + 1,dtype = int).tolist()
    
def indexes(series):
    seriesFirstInSeries = [x[0] for x in series]
    return np.argsort(seriesFirstInSeries).tolist()

#Combinations
Grid = []
# Loop over ingredients
for i in range(NP[0]):
    Grid.append([])
    #Loop over packets
    for j in range(NP[1]):
        #Grid
        Grid[-1].append(PacketServingRange(Packages[i][j],R[i]))
        if Grid[-1][-1] == [0]:
            del Grid[-1][-1]

#  Sort packet servings by servings per ingredient ASC
GridSorted = []   
for i in range(len(Grid)):
    GridSorted.append([])
    sortedindexes = indexes(Grid[i])
    for j in range(len(Grid[i])):
        GridSorted[-1].append(Grid[i][sortedindexes[j]])

GridResidual = GridSorted

Servings = 0
while min([len(x) for x in GridResidual]) != 0:
    match = False
    indexmin = np.argmin([j[0] for i in GridResidual for j in i])
    for n in range(len(GridResidual[0][0])):    
        if np.prod([GridResidual[0][0][n] in i[0] for i in GridResidual]):
            match = True
            Servings += 1
            for i in range(len(GridResidual)):
                del GridResidual[i][0]
            break
    if not match:
        del GridResidual[indexmin][0]
        
print('Servings - {}'.format(Servings))

