# [BFFs](https://code.google.com/codejam/contest/4304486/dashboard#s=p2)

## Problem

You are a teacher at the brand new Little Coders kindergarten. You have **N** kids in your class, and each one has a different student ID number from 1 through **N**. Every kid in your class has a single best friend forever (BFF), and you know who that BFF is for each kid. BFFs are not necessarily reciprocal -- that is, B being A's BFF does not imply that A is B's BFF.

Your lesson plan for tomorrow includes an activity in which the participants must sit in a circle. You want to make the activity as successful as possible by building the largest possible circle of kids such that each kid in the circle is sitting directly next to their BFF, either to the left or to the right. Any kids not in the circle will watch the activity without participating.

What is the greatest number of kids that can be in the circle? 

## Input

The first line of the input gives the number of test cases, **T**. **T** test cases follow. Each test case consists of two lines. The first line of a test case contains a single integer **N**, the total number of kids in the class. The second line of a test case contains **N** integers **F<subscript>1</subscript>**, **F<subscript>2</subscript>**, ..., **F<subscript>N</subscript>**, where **F<subscript>i</subscript>** is the student ID number of the BFF of the kid with student ID i. 

## Output

For each test case, output one line containing "Case #x: y", where x is the test case number (starting from 1) and y is the maximum number of kids in the group that can be arranged in a circle such that each kid in the circle is sitting next to his or her BFF. 

## Limits

* 1 ≤ **T** ≤ 100.
* 1 ≤ **F<subscript>i</subscript>** ≤ **N**, for all i.
* **F<subscript>i</subscript>** ≠ i, for all i. (No kid is their own BFF.)

### Small dataset

* 3 ≤ **N** ≤ 10.

### Large dataset

* 3 ≤ **N** ≤ 100.

## Sample

| Input | Output |
|-------|--------|
| 4 | |
| 4<br />2 3 4 1 | Case #1: 4 |
| 4<br />3 3 4 1 | Case #2: 3 |
| 4<br />3 3 4 3 | Case #3: 3 |
| 10<br />7 8 10 10 9 2 9 6 3 3  | Case #4: 6 |

In sample case #4, the largest possible circle seats the following kids in the following order: 7 9 3 10 4 1. (Any reflection or rotation of this circle would also work.) Note that the kid with student ID 1 is next to the kid with student ID 7, as required, because the list represents a circle. 