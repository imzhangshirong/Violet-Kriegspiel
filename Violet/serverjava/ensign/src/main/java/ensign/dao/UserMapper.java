package ensign.dao;

import ensign.pojo.User;

public interface UserMapper {
    int insert(User record);

    int insertSelective(User record);
}