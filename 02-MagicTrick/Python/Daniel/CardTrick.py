import pandas as pd

#Parse data from file and label it
#---------------------------------

#Read data in the pandas dataframe
input_data = pd.read_csv('C:\\Users\\DanielDiedericks\\Desktop\\Circle\\' \
                         'Magic Trick\\A-small-practice.in',header = None)
input_data.columns = ['Data']

#Determine number of cases and remove row
nr_of_cases = input_data.Data[0]
input_data = input_data[1:]

#Split data into columns
input_data = input_data['Data'].str.split(' ', expand=True)

#Add count of elements in row 
input_data['count_element'] = input_data.apply(lambda x : x.count(), axis = 1)

#Add determine wich rows belong to an answer set 
input_data['answer_set'] = input_data['count_element'].apply(lambda x : 1 \
          if x == 1 else 0)
input_data['answer_set'] = input_data['answer_set'].cumsum()
input_data['answer_set'] = input_data['answer_set'].apply(lambda x:\
          (x % 2) * -1 + 2)

#Determine which answer sets belong to a case
input_data['case_number'] = input_data['answer_set'].apply(lambda x: 1 \
          if x % 2 == 1 else 0)
input_data['case_number'] = input_data['case_number'] - \
          input_data['case_number'].shift(1)
input_data['case_number'] = input_data['case_number'].apply(lambda x: \
          1 if x == 1 else 0)  
input_data['case_number'] = input_data['case_number'].cumsum() + 1          

#Solve the problem
#-----------------

answer_data = pd.DataFrame(columns = ['case_number','answer'])
nr_of_cases = input_data['case_number'].max()

for x in range(1, nr_of_cases + 1):
    #Subset data into specific case
    subset_input_data = input_data[input_data.case_number == x]
    
    #Answer1
    #Extract the rownr from subset
    answer_1 = subset_input_data[0][(subset_input_data['count_element'] == 1)\
                                & (subset_input_data['answer_set'] == 1)]
    answer_1 = answer_1.reset_index(drop = True)
    answer_1 = int(answer_1[0])
    
    #Extract the cardlayout from subset
    arrangement_1 = subset_input_data[[0,1,2,3]]\
    [(subset_input_data['count_element'] == 4) \
     & (subset_input_data['answer_set'] == 1)]
    arrangement_1 = arrangement_1.reset_index(drop = True)
    
    #Obtain the first answer row
    answer_row_1 = arrangement_1[answer_1-1:answer_1].values.tolist()
    answer_row_1 = answer_row_1[0]    
    
    #Answer2
    #Extract the rownr from subset
    answer_2 = subset_input_data[0][(subset_input_data['count_element'] == 1) \
                                & (subset_input_data['answer_set'] == 2)]
    answer_2 = answer_2.reset_index(drop = True)
    answer_2 = int(answer_2[0])
    
    #Extract the cardlayout from subset
    arrangement_2 = subset_input_data[[0,1,2,3]]\
    [(subset_input_data['count_element'] == 4) \
     & (subset_input_data['answer_set'] == 2)]
    arrangement_2 = arrangement_2.reset_index(drop = True)
    
    #Obtain the second answer row
    answer_row_2 = arrangement_2[answer_2-1:answer_2].values.tolist()
    answer_row_2 = answer_row_2[0]     
    
    #find intersection of two rows
    overlap = [element for element in answer_row_1 if element in answer_row_2]
    
    #Compile the answer matrix
    #Create answer
    if len(overlap) == 0:
        y = 'Volunteer cheated!'
    elif len(overlap) == 1:
        y = overlap[0]
    elif len(overlap) > 1:
        y = 'Bad magician!'
    
    answer_data.loc[x-1] = ['Case #' +str(x),y]


