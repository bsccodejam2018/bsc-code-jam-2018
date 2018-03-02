#Import libraries
#-----------------

import random

#User defined variables
#----------------------

spatula_size = 3
nr_of_pancakes = 10

#User defined functions
#----------------------

#Function to generate a queue of pancakes represented by 1 and 0
def generate_pancakes(nr_of_pancakes = nr_of_pancakes):
    pancake_queue = []
    for i in range(nr_of_pancakes):
        pancake_queue.append(random.randint(0,1))
    return pancake_queue

#Function to find first 0
def find_first_upsidedown(pancake_queue):
    return pancake_queue.index(0)

#Function flip a set of pancakes by converting 1 to 0 and vice versa    
def flip_pancake(pancake_queue, starting_position,spatula_size = spatula_size):
    flip_check = False
    if starting_position + spatula_size > len(pancake_queue):
        return flip_check
    
    pancake_queue[starting_position:starting_position + spatula_size] = \
        [abs(p-1) for p \
         in pancake_queue[starting_position:starting_position + spatula_size]]
    flip_check = True
    return flip_check
    
#Implementation of code
#----------------------

#Generate pancake queue   
pancake_queue = generate_pancakes()

#Initiate counter
count = 0

#Flip pancakes
while True:
    print(pancake_queue)
    flip_check = False
    if 0 in pancake_queue:
        flip_check = flip_pancake(pancake_queue,find_first_upsidedown(pancake_queue))
    
    if flip_check == True:
        count += 1
    elif 0 not in pancake_queue:
        msg = "Succesfully solved in " + str(count) + " iterations"
        break
    else:
        msg = "Not solvable"
        break

print(msg)




