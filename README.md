In the console, go to the SolutionApi folder.

Then we execute ->  docker build -t api .

In the console, go to the WorkerBack folder. 

Then we execute ->  docker build -t back .

With these actions, we build images of the program in docker.

Then we run the following commands in the console to start all the components correctly.

docker network create myapp_net

docker run -d --network myapp_net --hostname rabbitmqhost --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management

docker run -d --network myapp_net -p 8082:80 --name api api

docker run -d --network myapp_net --name back back


To create tables and stored procedures in the MSSQL database, run the commands from the SQL.txt file.
