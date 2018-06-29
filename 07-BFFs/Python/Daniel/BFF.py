#@author: DanielDiedericks

import pandas as pd
import numpy as np
#import math as mth


#Find a connection function
def Find_BFFQueue(friend_list, visited_nodes, chain, kidnr):
    while True: 
        if len(chain) == 1:
            #initiate
            friendnr = min(visited_nodes.index[visited_nodes[0] != 'visited'])
        else:
            friendnr = int(friend_list[0][kidnr])
        
        chain.append(friendnr)
               
        if visited_nodes[0][friendnr] == 'visited':
            #function has reached it's conclusion return values
            
            #Check if in a '6' formation and removes the 'stem'
            if (len(chain) > 3) & (chain.index(friendnr) < len(chain) - 2) & \
            (chain[1] != friendnr):
                visited_nodes.loc[chain[1:chain.index(friendnr)]] = 0
                chain_t = []
                chain_t.append('start')
                chain_t = chain_t  + chain[chain.index(friendnr) : len(chain)]
                chain = chain_t.copy()
            
            #determine the chain type
            if (len(chain) > 4) & (chain[1] == friendnr):
                chain[0] = 'circle'
            elif (len(chain) == 4) & (chain[-3] == friendnr):
                chain[0] = 'recursive'
            else:
                chain[0] = 'dead'
                
            return chain
            break
        
        else:
            #Mark node as visited
            visited_nodes[0][friendnr] = 'visited'
        
        kidnr = friendnr
    
    #Walk backwards through dead chains to ensure longest chain has been 
    #selected

#Parse data from file and label it
#---------------------------------
        
#Columns -> Kid ID
#Rows -> Case ID

#Read data in the pandas dataframe
input_data = pd.read_csv('C:\\Users\\DanielDiedericks\\Desktop\\CodeJam\\' \
                         'C-small-practice.in',header = None)
input_data.columns = ['Data']

#Split data into columns
input_data = input_data['Data'].str.split(' ', expand=True)

#Add count of elements in row 
input_data['count_element'] = input_data.apply(lambda x : x.count(), axis = 1)

#Subset to only include data rows
input_data = input_data[input_data['count_element'] != 1]
input_data = input_data.reset_index(drop = True)

#Solve BFF
#-------------

#Generate output dataframe
output_data = pd.DataFrame(columns = range(0,2))

#Iterate over rows in input data
#for index, rows in input_data.iterrows():

index = 2                                 

#Slice dataframe to work only with current case
nr_kids = input_data.count_element[index]
original_kid_links = input_data.iloc[[index],0:nr_kids]
original_kid_links = original_kid_links.reset_index(drop = True)
original_kid_links = original_kid_links.transpose()
original_kid_links.index = np.arange(1,len(original_kid_links) + 1)

visited_kid_links = original_kid_links.copy()

#Iterate until all connections of nodes are found 
k = nr_kids - visited_kid_links[visited_kid_links[0] == 'visited'].count()
k = k[0]
longest_chain_r = 0
longest_chain_c = 0
chain = []

while k != 0 :
    #Find all the connecting BFF chains
    
    #Apply function to find a connecting string
    chain_t = []
    chain_t.append(['start'])
    chain_t = Find_BFFQueue(original_kid_links, visited_kid_links, chain_t, 1)
    
    chain.append(chain_t)
    
    k = nr_kids - visited_kid_links[visited_kid_links[0] == 'visited'].count()
    k = k[0]
    

#recursively ended BFF strings can sit next to other strings to form a circle
if chain[count_chain][0] == 'recursive':
    longest_chain_r += len(chain[count_chain]) - 1 

#completed loops cant be broken
if (chain[count_chain][0] == 'circle') & (len(chain[count_chain]) - 1 > longest_chain_c):
    longest_chain_c = len(chain[count_chain]) - 1 

longest_loop = max(longest_chain_c,longest_chain_r)
#figure six loops


#Add case number to output dataframe
