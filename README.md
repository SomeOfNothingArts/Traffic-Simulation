<b>Hello there! </b>

This is placeholder page for my Traffic simulation - package of simple scripts and test scene which can help with setting up living city feeling for your game. This is just a one element form one of my unfinished side project. It is not 100% functional as it should be but nothig really is.


<b>How it works:</b>

[![How it works](https://img.youtube.com/vi/f3ACMRRVh2E/0.jpg)](https://www.youtube.com/watch?v=f3ACMRRVh2E)


<b>How use automatic points connector:</b>

Due to laziness and lack of patience to assign every reference to navigation point i made button which make that automatically. It checks if any part of road in specific range near him have collider + nav point as a child and just connects choosen element in array as reference (you can ctrl+z and click again how many times you want couse it keeps focus on active point and keeps track of last connected nav point id).

[![How use automatic connector](https://img.youtube.com/vi/YVhW97ALJdU/0.jpg)](https://www.youtube.com/watch?v=YVhW97ALJdU)

<b>Navigation point:</b>


![alt text](https://raw.githubusercontent.com/SomeOfNothingArts/Traffic-Simulation/main/img3.png)

Selected Navi Point - which element of next navigation points array will be replaced with autoconnector.

Auto Connector Range - range in which this navi point searches for next navi points with auto connector.

Next Navigation Points - list of possible next navigation points for incoming cars (they are picked with higher chance for 1st ).

Occupying Object - car (if any) which acctually occupy this navigation point.

Red Light - variable which forbids cars from going foward (controlled by traffic light).

Is End Point - if true cars will despawn after reaching it.

Is Start Point - if true works as spawn point for new cars.

Is Crossing Start Point - if true this point forbids cars to collide with each other (works great witch traffic light and keeps crossing clean).

Is Crossing End Point - if true it enable incoming cars to collide with each other again.

<b>/// Assets from those videos are replaced with something simpler and they are looking something like this:</b>

![alt text](https://raw.githubusercontent.com/SomeOfNothingArts/Traffic-Simulation/main/img1.png)
