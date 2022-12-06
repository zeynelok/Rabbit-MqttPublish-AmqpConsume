## Docker RabbitMQ Configuration

- Run docker containers 

> docker run -d --rm --net rabbits -p 8081:15672 -p 1881:1883 -p 5671:5672 -e RABBITMQ_ERLANG_COOKIE='rabbitcookie' --hostname rabbit-1 --name rabbit-1 rabbitmq:3.11.2-management

> docker run -d --rm --net rabbits -p 8082:15672 -p 1882:1883 -p 5672:5672 -e RABBITMQ_ERLANG_COOKIE='rabbitcookie' --hostname rabbit-2 --name rabbit-2 rabbitmq:3.11.2-management

> docker run -d --rm --net rabbits -p 8083:15672 -p 1883:1883 -p 5673:5672 -e RABBITMQ_ERLANG_COOKIE='rabbitcookie'  --hostname rabbit-3 --name rabbit-3 rabbitmq:3.11.2-management

- Enable MQTT plugin
> docker exec -it rabbit-1 bash

> rabbitmq-plugins enable rabbitmq_mqtt

- Cluster 

> docker exec -it rabbit-2 rabbitmqctl stop_app

> docker exec -it rabbit-2 rabbitmqctl reset

> docker exec -it rabbit-2 rabbitmqctl join_cluster rabbit@rabbit-1

> docker exec -it rabbit-2 rabbitmqctl start_app

> docker exec -it rabbit-2 rabbitmqctl cluster_status
