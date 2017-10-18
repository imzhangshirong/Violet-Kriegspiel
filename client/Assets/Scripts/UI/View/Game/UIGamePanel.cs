using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIGamePanel : UIViewBase
{
    public GameObject m_MyselfFied;
    public GameObject m_EnemyFied;

    private GameObject m_ChessHero;
    public override void OnInit()
    {
        base.OnInit();
        AppInterface.UIManager.HideOverViewByPage("UITopTest");

        //加载
        m_ChessHero = AppInterface.ResourceManager.LoadUI("Game/ChessHero");

        //注册消息事件
        BindEvent("_chessClick", OnChessClick);
        BindEvent("_chessHeroClick", OnChessHeroClick);
    }
    public override void OnOpen(Intent intent)
    {
        base.OnOpen(intent);
        Debuger.Log("GamePanel Open");
        PutChessHero(0);
        PutChessHero(1);
    }
    public void BackPage()
    {
        AppInterface.UIManager.PageBack();
    }
    public override void OnRefresh()
    {
        base.OnRefresh();
        Debuger.Log("GamePanel Refresh");
    }
    private void OnChessClick(object content)
    {
        Intent intent = (Intent)content;
        int id = (int)intent.Value("id");
        GameObject go = (GameObject)intent.Value("gameObject");
        Debuger.Log("ChessItem Click:"+id);
    }

    private void OnChessHeroClick(object content)
    {
        Intent intent = (Intent)content;
        int id = (int)intent.Value("id");
        GameObject go = (GameObject)intent.Value("gameObject");
        Debuger.Log("ChessHeroItem Click:" + id);
        
    }

    private void PutChessHero(int type)
    {
        TreeRoot treeRoot = GetComponent<TreeRoot>();
        int baseId = type * 100;
        for(int i = 0; i < 30; i++)
        {
            if(CanDefaultPut(type,i))
            {
                GameObject go = Instantiate(m_ChessHero);
                
                
                ChessHeroItem chessHeroItem = go.GetComponent<ChessHeroItem>();
                chessHeroItem.treeRoot = treeRoot;
                treeRoot.Bind(chessHeroItem);//绑定到TreeRoot
                chessHeroItem.chessId = baseId + i;
                switch (type)
                {
                    case 0:
                        go.transform.parent = m_MyselfFied.transform;
                        chessHeroItem.state = ChessHeroState.Show;
                        break;
                    case 1:
                        go.transform.parent = m_EnemyFied.transform;
                        chessHeroItem.state = ChessHeroState.Hide;
                        break;
                }
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(-256 + i % 5 * 128, (i / 5) * 64, 0);
                go.SetActive(true);


            }
        }
    }
    private bool CanDefaultPut(int type,int id)
    {
        switch (type)
        {
            case 0:
                if (id != 11 && id != 13 && id != 17 && id != 21 && id != 23) return true;
                break;
            case 1:
                if (id != 6 && id != 8 && id != 12 && id != 16 && id != 18) return true;
                break;
        }
        return false;
    }
}
