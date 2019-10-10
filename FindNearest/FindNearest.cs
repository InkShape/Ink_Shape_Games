using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNearest : MonoBehaviour {

    public class Target {
        string name;
        float distance;
        public GameObject obj;
        public Target(string name, float distance, GameObject obj) {
            this.name = name;
            this.distance = distance;
            this.obj = obj;
        }

        public string getName() {
            return name;
        }
        public float getDistance() {
            return distance;
        }
        
    } // Target Class

    public bool debugMode;
    public bool debugGui;
    public LayerMask detectMask; // LayerMask of Targets (Create a Layer "Target" if you prefer) it must be diferent for this object layer
    public float detectRadio;
    [Range(0.01f,5)]
    public float refreshTime; // Refresh Lists to look for nearest
    public List<Collider> collisions = new List<Collider>();
    public List<Target> targets_ = new List<Target>();


    private void Start() {
        StartCoroutine(FindNearestLoop());
    }

    IEnumerator FindNearestLoop() {
        while (true) {
            yield return new WaitForSeconds(refreshTime);
            collisions.Clear();
            targets_.Clear();
            LookForNearest();
        }
    }

    void LookForNearest() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadio, detectMask.value); // Get Colliders 
        if (colliders.Length > 0) {
            foreach (Collider col in colliders) {
                if (!collisions.Contains(col)) {
                    collisions.Add(col);        // Add Colider to List
                    float distance = Vector3.Distance(gameObject.transform.position, col.transform.position); // Get distance between this object and target
                    targets_.Add(new Target(gameObject.name, distance, col.gameObject));        // Add target to list
                    targets_.Sort(delegate (Target a, Target b) {       // Sort List to minor distance to max
                        return a.getDistance().CompareTo(b.getDistance());
                    });
                }
            }
            if (debugMode) {
                foreach (Target target in targets_) {
                    print(target.getName() + " " + target.getDistance());
                    target.obj.GetComponent<Renderer>().material.color = Color.white;
                }

                targets_[0].obj.GetComponent<Renderer>().material.color = Color.red;
            }
        }

    }

    private void OnDrawGizmos() {
        if (debugGui) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRadio);
            if (targets_.Count > 0) {
                Gizmos.DrawLine(transform.position, targets_[0].obj.transform.position);
            }
        }
    }
}


