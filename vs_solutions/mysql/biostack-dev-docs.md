# MySQL development docs
Mysql database field attributes notes:

> AI: Auto Increment; B: Binary; NN: Not Null; PK: Primary Key; UQ: Unique; UN: Unsigned; ZF: Zero Fill

## app
The analysis application that running the task

|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``AI``, ``NN``||
|name|VarChar (128)|``NN``||
|description|Text|||
|catagory|VarChar (45)|||

```SQL
CREATE TABLE `app` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL,
  `description` longtext,
  `catagory` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`uid`),
  UNIQUE KEY `uid_UNIQUE` (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='The analysis application that running the task';
```



## subscription


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``AI``, ``NN``||
|email|VarChar (128)|``NN``||
|hash|VarChar (64)|``NN``||
|app|Int64 (11)|``NN``||
|active|Int64 (11)|``NN``|1 OR 0|

```SQL
CREATE TABLE `subscription` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(128) NOT NULL,
  `hash` varchar(64) NOT NULL,
  `app` int(11) NOT NULL,
  `active` int(11) NOT NULL DEFAULT '0' COMMENT '1 OR 0',
  PRIMARY KEY (`email`),
  UNIQUE KEY `uid_UNIQUE` (`uid`),
  KEY `fk_subscription_app1_idx` (`app`),
  CONSTRAINT `fk_subscription_app1` FOREIGN KEY (`app`) REFERENCES `app` (`uid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



## task_errors
Task executing errors log

|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``NN``||
|app|Int64 (11)|``NN``|The task app name|
|task|Int64 (11)|``NN``|The task uid|
|exception|Text|``NN``|The exception message|
|type|VarChar (45)|``NN``|GetType.ToString|
|stack-trace|VarChar (45)|``NN``||
|solved|Int64 (11)|``NN``|这个bug是否已经解决了？ 默认是0未解决，1为已经解决了|

```SQL
CREATE TABLE `task_errors` (
  `uid` int(11) NOT NULL,
  `app` int(11) NOT NULL COMMENT 'The task app name',
  `task` int(11) NOT NULL COMMENT 'The task uid',
  `exception` longtext NOT NULL COMMENT 'The exception message',
  `type` varchar(45) NOT NULL COMMENT 'GetType.ToString',
  `stack-trace` varchar(45) NOT NULL,
  `solved` int(11) NOT NULL DEFAULT '0' COMMENT '这个bug是否已经解决了？ 默认是0未解决，1为已经解决了',
  PRIMARY KEY (`uid`,`app`),
  KEY `fk_task_errors_app1_idx` (`app`),
  CONSTRAINT `error_task` FOREIGN KEY (`app`) REFERENCES `task_pool` (`uid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_task_errors_app1` FOREIGN KEY (`app`) REFERENCES `app` (`uid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Task executing errors log';
```



## task_pool
这个数据表之中只存放已经完成的用户任务信息

|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``AI``, ``NN``||
|md5|VarChar (32)|``NN``|用户查询任务状态结果所使用的唯一标识符字符串|
|workspace|Text||保存数据文件的工作区文件夹|
|time_create|DateTime||这个用户任务所创建的时间|
|time_complete|DateTime||这个用户任务所完成的时间|
|result_url|Text||结果页面的url|
|email|VarChar (45)||任务完成之后通知的目标对象的e-mail,如果不存在，则不发送email|
|title|VarChar (128)||任务的标题（可选）|
|description|Text||任务的描述(可选)|
|status|Int64 (11)||任务的结果状态\n\n-100 任务执行失败\n1 任务成功执行完毕\n0 任务未执行或者执行中未完毕|
|app|Int64 (11)|``NN``|The task app id|
|parameters|Text|``NN``|使用json保存着当前的这个任务对象的所有的构造函数所需要的参数信息|

```SQL
CREATE TABLE `task_pool` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `md5` varchar(32) NOT NULL COMMENT '用户查询任务状态结果所使用的唯一标识符字符串',
  `workspace` mediumtext COMMENT '保存数据文件的工作区文件夹',
  `time_create` datetime DEFAULT NULL COMMENT '这个用户任务所创建的时间',
  `time_complete` datetime DEFAULT NULL COMMENT '这个用户任务所完成的时间',
  `result_url` mediumtext COMMENT '结果页面的url',
  `email` varchar(45) DEFAULT NULL COMMENT '任务完成之后通知的目标对象的e-mail,如果不存在，则不发送email',
  `title` varchar(128) DEFAULT NULL COMMENT '任务的标题（可选）',
  `description` mediumtext COMMENT '任务的描述(可选)',
  `status` int(11) DEFAULT NULL COMMENT '任务的结果状态\n\n-100 任务执行失败\n1 任务成功执行完毕\n0 任务未执行或者执行中未完毕',
  `app` int(11) NOT NULL COMMENT 'The task app id',
  `parameters` longtext NOT NULL COMMENT '使用json保存着当前的这个任务对象的所有的构造函数所需要的参数信息',
  PRIMARY KEY (`uid`),
  UNIQUE KEY `md5_UNIQUE` (`md5`),
  UNIQUE KEY `uid_UNIQUE` (`uid`),
  KEY `fk_task_pool_app1_idx` (`app`),
  CONSTRAINT `fk_task_pool_app1` FOREIGN KEY (`app`) REFERENCES `app` (`uid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='这个数据表之中只存放已经完成的用户任务信息';
```



## visitor_stat


|field|type|attributes|description|
|-----|----|----------|-----------|
|uid|Int64 (11)|``AI``, ``NN``||
|time|DateTime|``NN``||
|ip|VarChar (45)|``NN``||
|url|Text|``NN``|Url that going to visit this web site|
|success|Int64 (11)|``NN``||
|method|VarChar (45)||GET/POST/PUT.....|
|ua|VarChar (1024)||User agent|
|ref|Text||reference url, Referer|
|data|Text||additional data notes|
|app|Int64 (11)|``NN``||

```SQL
CREATE TABLE `visitor_stat` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `time` datetime NOT NULL,
  `ip` varchar(45) NOT NULL,
  `url` tinytext NOT NULL COMMENT 'Url that going to visit this web site',
  `success` int(11) NOT NULL,
  `method` varchar(45) DEFAULT NULL COMMENT 'GET/POST/PUT.....',
  `ua` varchar(1024) DEFAULT NULL COMMENT 'User agent',
  `ref` mediumtext COMMENT 'reference url, Referer',
  `data` mediumtext COMMENT 'additional data notes',
  `app` int(11) NOT NULL,
  PRIMARY KEY (`ip`,`time`,`app`),
  UNIQUE KEY `uid_UNIQUE` (`uid`),
  KEY `fk_visitor_stat_app1_idx` (`app`),
  CONSTRAINT `fk_visitor_stat_app1` FOREIGN KEY (`app`) REFERENCES `app` (`uid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
```



