# Dynamic-Room

This README was first written by Yan on 2019/8/2.

## Abstract

This project is tring to provide physical feedback for virtual room structures in room-scale VR. The method we use is navigating a set of robotic walls to match the position and orientation of the virtual structure the user is going to touch and encounter the user's touch. All the robotic walls and the user is tracked by a tracking system. User can explore a 3 m × 4 m 4-wall room with physical feedback through 3 or 2 robotic walls. And we also provide a simulation without robotic walls to show how this system work. In this project, we achieve this system through 2 different algorithm, Detection Algorithm and Machine Learning Combined Algorithm. We provide example scene for every algorithms. 

## Collaborators
- Current
  - Yan Yixian
- Past
  - Nobody

# Software Version
- **Unity 2018.3.11f1**
- IDE (Visual Studio 2017)
- Python 2.7

Unity plugin / Unity Asset
- Final IK 1.6
- SteamVR 1.0
- NetMQ 4.0.0.207
- AsyncIO 0.1.69

Python Library
- zmq
- numpy
- pandas
- sklearn
- joblib

# Hardware Version
VR Devices
- HTC Vive
- TPCAST Wireless Adapter for Vive

Robotic Wall
- Actuator
  - IRobot Roomba 600 Series
- Overview
  ![image](img/proto_device.png)

# Structure of project

## Scenes

| Scene Name | Build Target | Description |
----|----|---- 
| *Real_Detection* | Windows | 4-wall room with Detection Algorithm and real robotic walls. |
| *Simulation_Detection* | Windows | 4-wall room with Detection Algorithm and simulated robotic walls. |
| *Real_ML* | Windows | 4-wall room with ML Combined Algorithm and real robotic walls. |
| *Simulation_ML* | Windows | 4-wall room with ML Combined Algorithm and simulated robotic walls. |

## Class
### Class Explanatory Text
#### Common Scripts
State Machine

| Class Name | Attached GameObject |Description (What does the class do?) |
----|----|---- 
| *WallState* | Animator.Wall(state) | Define the behaviour of robotic wall which is materializing virtual structure. |
| *Standby_State* | Animator.Standby(state) | Define the behaviour of robotic wall which is not materializing virtual structure and in standby. |

RVO Obstacle Avoidance

| Class Name | Attached GameObject |Description (What does the class do?) |
----|----|---- 
| *RVO_agent* | robotic wall | According to the goal position, plan the path without collsion for robotic wall, and send the waypoint to PID controller classes. |
| *RVO_user_agent* | user avatar | Set user as a special RVO agent which's can't be controlled to make the robotic walls avoid user. |
