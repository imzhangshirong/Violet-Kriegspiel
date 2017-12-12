package ensign.pojo;

public class User {
    private Integer id;

    private String userName;

    private String password;

    private String token;

    public User(Integer id, String userName, String password, String token) {
        this.id = id;
        this.userName = userName;
        this.password = password;
        this.token = token;
    }

    public User() {
        super();
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }
}