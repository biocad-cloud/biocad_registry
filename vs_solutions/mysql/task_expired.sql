CREATE DEFINER=`root`@`localhost` FUNCTION `task_expired`(time_complete datetime) RETURNS tinyint(1)
BEGIN

   /* 
    * 用户的任务执行结果数据只保存24个小时，则在这个函数之中需要进行判断的是
    * 任务的完成时间和现在的时间差是否大于24个小时？ 
    *
    * 如果是，则说明任务已经过期了，则会返回true删除数据
    * 如果不是，则返回false
    */
   DECLARE val integer;

   SET val = TIMESTAMPDIFF(HOUR, time_complete, now()) ;
   RETURN val >= 24;

END