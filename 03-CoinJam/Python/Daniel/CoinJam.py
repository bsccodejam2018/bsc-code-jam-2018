#@author: DanielDiedericks

import pandas as pd
import numpy as np
import math as mth

#Parse data from file and label it
#---------------------------------

#Read data in the pandas dataframe
input_data = pd.read_csv('C:\\Users\\DanielDiedericks\\Desktop\\Circle\\' \
                         'CoinJam\\C-small-practice.in',header = None)
input_data.columns = ['Data']

#Split data into columns
input_data = input_data['Data'].str.split(' ', expand=True)

#Rename columns
input_data.columns = ['coin_length','nr_solutions']

#Add count of elements in row 
input_data['count_element'] = input_data.apply(lambda x : x.count(), axis = 1)

#Add column to determine wich test cases belong to a coin set 
input_data['coin_set'] = input_data['count_element'].apply(lambda x : 1 \
          if x == 1 else 0)
input_data['coin_set'] = input_data['coin_set'].cumsum()

#Label test cases
input_data['testcase_number'] = input_data.loc[input_data['count_element'] \
           == 1,'coin_length']
input_data['testcase_number'] = input_data['testcase_number'].fillna(0)
input_data['testcase_number'] = input_data['testcase_number'].\
    apply(lambda x: int(x) * -1 if x != 0 else 1)
input_data['testcase_number'] = input_data['testcase_number'].cumsum()       
input_data['temp'] = input_data.where(input_data['count_element'] == 1).\
    groupby('coin_set')['coin_length'].transform('max').fillna(0).\
    astype('int32')
    #Note functions here for future use    
input_data['temp'] = input_data.groupby('coin_set')['temp'].transform('max')
input_data['testcase_number'] = input_data['testcase_number'] \
    + input_data['temp']

#Delete temp rows
del input_data['temp'] 
del input_data['count_element']

#Subset to only include data rows
input_data = input_data[input_data['testcase_number'] != 0]
input_data = input_data.reset_index(drop = True)

#Convert columns to int
input_data = input_data.astype(np.int64)

#Solve coinjam
#-------------

#Generate output dataframe
output_data = pd.DataFrame(columns = range(0,10))

#Iterate over rows in input data
for index, rows in input_data.iterrows():
    
    #Add case number to output dataframe
    output_data = output_data.append(['Case #' + str(index + 1)])
    
    #Generate the max number up to which a coinjam can be generated
    maxbin = 2**(rows['coin_length']-2) - 1
    original_dec_integer = 0
    found = 0
    
    #Iterate until set number of solutions are found
    #Reaching maxbin arbitrary but serves as a failsafe
    while (found <= rows['nr_solutions']) and (original_dec_integer <= maxbin):        
        
        #Convert to a binary string
        bin_value = bin(original_dec_integer)
        bin_value = bin_value[-(len(bin_value)-2):]
        bin_value = '0' * (rows['coin_length'] - len(bin_value)) + bin_value
        bin_value = '1' + bin_value[-(len(bin_value)-2):] + '1'
        
        #Initiate divisor list
        divisor_list = list()
        
        #Iterate over the range of values to interpret the binary number as
        for m in range(2,11):
            #Initiate empty divisor list 
            #Reinterpret the binary string to base k
            new_dec_integer = int(bin_value,m)
            
            #Find a divisor for the new integer
            found_divisor_flag = 0
            for n in range(2,mth.floor(new_dec_integer**0.5)):
                if new_dec_integer % n == 0:
                    found_divisor_flag = 1
                    divisor_list.append(n)
                    break
            
            if found_divisor_flag == 0:
                break
        
        #Add solution to output data
        if len(divisor_list) >= 9:
            found += 1
            #divisor_list = np.asanyarray(divisor_list)
            divisor_list = [bin_value] + divisor_list
            output_data = output_data.append([divisor_list])
                
        original_dec_integer += 1 

#Condition output dataframe into the right format
output_data.reset_index(drop = True)
output_data = output_data.fillna('')
output_data = output_data.astype(str)
output_data['output'] = output_data.apply(' '.join, axis=1)
output_data = output_data['output']
