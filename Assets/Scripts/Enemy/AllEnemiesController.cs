using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesController : MonoBehaviour
{
    public GameController gameController;
    private List<GameObject> allChildren = new List<GameObject>();
    void Start()
    {
        allChildren = GetAllChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if (allChildren.Count <= 0 ) {
            gameController.Victory();
        }
    }

    public List<GameObject> GetAllChildren() {
        List<GameObject> gs = new List<GameObject>();
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        if (ts == null) {
            return gs;
        }

        foreach (Transform t in ts) {
            if (t != null && t.gameObject != null) {
                if (t.gameObject.TryGetComponent<EnemyController>(out var enemy)) {
                    if (!enemy.isDead) {
                        gs.Add(t.gameObject);
                    }
                }
            }
        }
        return gs;
    }

    public void KillChildren(GameObject go) {
        allChildren.Remove(go);
    }
}
