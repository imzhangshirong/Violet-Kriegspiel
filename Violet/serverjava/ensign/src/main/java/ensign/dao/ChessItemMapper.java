package ensign.dao;

import ensign.pojo.ChessItem;

public interface ChessItemMapper {
    int insert(ChessItem record);

    int insertSelective(ChessItem record);

    ChessItem selectByPrimaryKey(Integer id);

    int updateByPrimaryKeySelective(ChessItem record);

    int updateByPrimaryKey(ChessItem record);
}