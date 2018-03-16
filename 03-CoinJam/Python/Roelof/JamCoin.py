# -*- coding: utf-8 -*-
"""
Created on Fri Mar  9 09:05:09 2018

@author: RoelofSchoeman
"""
#  Coinjam code in Python3

import numpy as np

N = 30
J = 500
maxit = 10

def Base2(Value,N):
    ## Evaluate the Base2 string expression for the coin
    binary = '{0:b}'.format(Value)
    s = ''
    for i in range(N-len(binary)-2):
        s += '0'
    s += binary
    return s

def BaseValue(s):
    ## Evaluate the 9 base values of an input string
    Values = []
    strn = str(int(s))[::-1]
    for k in [2,3,4,5,6,7,8,9,10]:
        Values.append(sum([int(strn[i])*k**i for i in range(len(strn))][::-1]))
    return Values

def divisor(n):
    ## Evalute a divisor for a maximum of maxint iterations else return 0
    ## which indicates a possible prime
    if n == 2 or n == 3: return 0
    if n < 2 or n%2 == 0: return 2
    if n < 9: return 0
    if n%3 == 0: return 3
    r = int(n**0.5)
    f = 5
    count = 0
    while (f <= r) & (count < maxit):
      if n%f == 0: return f
      if n%(f+2) == 0: return f+2
      f +=6
      count += 1
    return 0

## Alogirithm     
coin = []
coindivisor = []
j = 0   
n = -1
while (n < 2**(N-2)) & (j < J):
    notJam = False
    n += 1
    s = '1'
    s += Base2(n,N)
    s += '1'
#    print(n)
    Values  = BaseValue(s)
    divisorlist = []
    for Value in Values:
        div = divisor(Value)
        if div == 0:
            notJam = True
            break
        divisorlist.append(div)
    if notJam:
        continue
    coin.append(s)
    coindivisor.append(divisorlist)
    print('{}-{}'.format(coin[-1],coindivisor[-1]))
    j += 1