using UnityEngine;

public class SwipeTrail : MonoBehaviour {

    public Enemy enemy;
    private float lastAtk;

    void Start()
    {
        lastAtk = Time.time;
    }
    
	void Update () {
        if (Input.GetMouseButton(0) || 
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            Plane objPlane = new Plane(Camera.main.transform.forward*-1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                this.transform.position = mRay.GetPoint(rayDistance);

                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit.collider != null && Time.time - lastAtk > 0.3f)
                {
                    print(Time.time - lastAtk);
                    lastAtk = Time.time;
                    SoundManager.instance.SwordAttack();
                    enemy.DamageEnemy();
                }
            }
        }
    }
}
