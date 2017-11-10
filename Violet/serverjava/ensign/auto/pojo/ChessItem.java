package pojo;

public class ChessItem {
    private Integer id;

    private Integer chessid;

    private Integer type;

    private String position;

    public ChessItem(Integer id, Integer chessid, Integer type, String position) {
        this.id = id;
        this.chessid = chessid;
        this.type = type;
        this.position = position;
    }

    public ChessItem() {
        super();
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getChessid() {
        return chessid;
    }

    public void setChessid(Integer chessid) {
        this.chessid = chessid;
    }

    public Integer getType() {
        return type;
    }

    public void setType(Integer type) {
        this.type = type;
    }

    public String getPosition() {
        return position;
    }

    public void setPosition(String position) {
        this.position = position;
    }
}