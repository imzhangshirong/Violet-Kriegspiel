package dao;

import pojo.User;

public interface UserMapper {
    int insert(User record);

    int insertSelective(User record);
}