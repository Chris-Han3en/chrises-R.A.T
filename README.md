# NOTICE
This is for educational purposes only, i take no responsibility for any damage caused, nor any responsibility of the actions of others who decide to use the code.
# How to use
download the files and open the sin project file, there might be a few errors due to file locations and names but they should be easily fixed. When into the project build the project with 'dotnet build' or 'dotnet run' and see if there are any errors. You can open the client or the server first they wait for each other before making a connection.
# Commands
There a few built in commands such as
* Internet disonnection
* Blue Screen of Death
* Mario/tetris theme sounds which play through the console
* PC Shutdown command

The foundation is there and you can add whatever commands you want.

# How to add commands
Both the client and the server have .cs files named commands which when you enter you can see. To add new command for the server you have to put an if statement with the words you want to put to start the command then send that command over the server (look at defult commands for example), you also have to add the command names at the top of the file otherwise it wont recognize the command. Then to go client and then you put an if statement saying if the command recieved contains or is equal to the keyword then to run the cs code. (look at defult commands for example)

# Errors
Any errors you might encounter i will try my best to answer and fix the problem but i will ignore any stupid questions such as "How do i complile the code?"

# Made by chris hansen#3106
