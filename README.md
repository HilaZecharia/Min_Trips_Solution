# Min Trip Solution - Read Me 
I implemented the solution in wpf application 
when execute the project will open window with text box to input the number of bags and after it need to click on "Enter weights" button 
and will display dynamicly text box for each bag (X number of bags the user entered before)
after it click on the button "calculate trip" and the result will display in new window.

the logic is on file : TripCalculation.cs

Way of thinking :
I choose to implement the creation of the elements  dynamicly
for entering the weights in a separate task in order not to "freezing" the UI for large inputs values.
In addition, I choose to perform the process of calculating the shortest required
 trips asynchronously in order not to maintain scalability and responsiveness.

for the logic and trip calcuation I perfurm the "greedy approch" because we want to "maximizing weight" by 
taking the smallest items first, this is the most efficient way to utilize the available space in the bags so 
If an item fits in the remaining capacity, you take it. 
 
Time Complexity: O(n log(n)) for the sorting.
