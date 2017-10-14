using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour {
    GameObject[] cubes = new GameObject[4];
    GameObject[] gos = new GameObject[4];
    float fog = 0;//霧

    // Use this for initialization
    void Start()
    {
        RenderSettings.fog = true; //霧ON,OFF
        RenderSettings.fogColor = Color.gray; //霧色
        RenderSettings.fogDensity = 0f; //霧密度
        for (int i = 0; i < 4; i++)
        {
            cubes[i] = GameObject.Find("Cube" + i);
            gos[i] = GameObject.Find("GameObject" + i);
        }
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject obj in objs)
        {
            if(obj.name != "Sphere")
            {
                Color c = obj.GetComponent<Renderer>().material.color;
                c.a = 0.5f;
                obj.GetComponent<Renderer>().material.shader = Shader.Find("GUI/TextShader");
            }
        }

    }    

	
	
	// Update is called once per frame
	void Update () {
        if(RenderSettings.fogDensity < fog)
        {
            RenderSettings.fogDensity += 0.0001f; //霧の密度をupdateごとに0.0001増やす
        }

        foreach(GameObject obj in cubes)
        {
            obj.transform.Rotate(new Vector3(1f, 1f, 1f));
        }
        Vector3 v = transform.position;
        v.y += 2;
        v.z -= 7;
        Camera.main.transform.position = v;

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-1f, 0, 1f));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(1f, 0, -1f));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(1f, 0, 1f));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-1f, 0, -1f));
        }
        AddForceAll(); //操作を操作
    }
    //操作からボールの後ろを追跡するメソッド
    private void AddForceAll()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("sphere"); //sphereタグのオブジェクトを取り出す
        foreach(GameObject obj in objs)
        {
            Vector3 dir = transform.position - obj.transform.position; //自分の位置から相手の位置を引くことで、「相手の位置から自分の位置に向かうベクトルを得る」
            obj.GetComponent<Rigidbody>().AddForce(dir * 0.1f); //1/10に弱めてAddForce
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name.StartsWith("Cube"))
        {
            for(int i = 0; i < 4; i++)
            {
                if(cubes[i] == collider.gameObject)
                {
                    ParticleSystem ps = gos[i].GetComponent("ParticleSystem") as ParticleSystem;
                    ps.Play();
                    Vector3 p = cubes[i].transform.position; //cubeの位置取得
                    cubes[i].SetActive(false);
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere); //オブジェクトの作成でSphereを選択
                    obj.AddComponent<Rigidbody>(); //剛体特性を組み込む
                    obj.transform.position = p; //接触したcubeの位置を保存
                    obj.GetComponent<Renderer>().material.color = Color.cyan; //色の変更
                    obj.tag = "sphere"; //オブジェクトにsphereタグをつける
                    fog += 0.05f; //cubeに当たるごとに霧の密度を追加
                }
            }
        }
        if(collider.gameObject.name.StartsWith("Cylinder"))
        {
            Behaviour b = collider.gameObject.GetComponent("Halo") as Behaviour;
            b.enabled = true;
        }
    }
}
