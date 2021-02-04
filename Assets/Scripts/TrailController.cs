using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject Trail;
    public Bird TargetBird;

    // trails yang akan ditampilkan
    private List<GameObject> _trails;

    // Start is called before the first frame update
    void Start()
    {
        _trails = new List<GameObject>();
    }

    // menambahkan burung yang akan dijadikan target, kemudian me reset ulang trail yang ada
    public void SetBird(Bird bird)
    {
        TargetBird = bird;
        for(int i = 0; i < _trails.Count; i++)
        {
            Destroy(_trails[i].gameObject);
        }

        _trails.Clear();
    }

    // membuat game object trail setiap 100 ms
    public IEnumerator SpawnTrail()
    {
        
        _trails.Add(Instantiate(Trail, TargetBird.transform.position, Quaternion.identity));

        yield return new WaitForSeconds(0.1f);

        if(TargetBird != null && TargetBird.state != Bird.BirdState.HitSomething)
        {
            StartCoroutine(SpawnTrail());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
