using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public Collider2D viewport;

    float boundsX;
    float boundsY;

    public GameObject ramPrefab;
    public GameObject turretPrefab;
    public GameObject laserPrefab;

    public List<WaveEvent> waveEvents;

    public List<LerpingEntity> lerpingEntities = new List<LerpingEntity>();

    float lerpTime = 1f;

    public List<GameObject> instances = new List<GameObject>();
    public List<bool> constistencies = new List<bool>();

    List<GameObject> required = new List<GameObject>();

    void Start() {
        boundsX = viewport.bounds.extents.x;
        boundsY = viewport.bounds.extents.y;
    }

    void Update() {
        for (int i = 0; i < lerpingEntities.Count; i++) {
            float time = lerpingEntities[i].timePassed;
            time += Time.deltaTime;
            lerpingEntities[i].entity.transform.position = Vector2.Lerp(lerpingEntities[i].startPos, lerpingEntities[i].goalPos, time / lerpTime);
            if (time > lerpTime) {
                lerpingEntities[i].entity.transform.position = lerpingEntities[i].goalPos;
                lerpingEntities[i].entity.GetComponent<Animator>().SetTrigger("Charge");
                lerpingEntities.RemoveAt(i);
            } else {
                lerpingEntities[i] = new LerpingEntity(lerpingEntities[i].entity, lerpingEntities[i].startPos, lerpingEntities[i].goalPos, time);
            }
        }
        List<GameObject> trueRequired = new List<GameObject>();
        for (int i = 0; i < required.Count; i++) {
            if (required[i] != null) {
                trueRequired.Add(required[i]);
            }
        }
        required = trueRequired;
    }

    public void StartWaves() {
        StartCoroutine(WaveCoroutine());
    }

    IEnumerator WaveCoroutine() {
        foreach (WaveEvent waveEvent in waveEvents) {
            foreach (SpawnEvent spawnEvent in waveEvent.spawnEvents) {
                if (spawnEvent.type != EventType.Loop) {
                    GameObject entity = SpawnEntity(spawnEvent);
                    instances.Add(entity);
                    constistencies.Add(spawnEvent.consistent);
                    if (spawnEvent.required) { required.Add(entity); }
                    if (spawnEvent.waitForInitAnimation) {
                        yield return new WaitForSeconds(spawnEvent.type == EventType.Ram ? 0 : lerpTime);
                    }
                    yield return new WaitForSeconds(spawnEvent.delay);
                } else {
                    for (int i = 0; i < spawnEvent.loop.amount; i++) {
                        for (int j = spawnEvent.loop.start; j < spawnEvent.loop.end; j++) {
                            GameObject entity = SpawnEntity(waveEvent.spawnEvents[j]);
                            instances.Add(entity);
                            constistencies.Add(waveEvent.spawnEvents[j].consistent);
                            if (waveEvent.spawnEvents[j].required) { required.Add(entity); }
                            if (waveEvent.spawnEvents[j].waitForInitAnimation) {
                                yield return new WaitForSeconds(waveEvent.spawnEvents[j].type == EventType.Ram ? 0 : lerpTime);
                            }
                            yield return new WaitForSeconds(waveEvent.spawnEvents[j].delay);
                        }
                    }
                }
            }
            if (waveEvent.progressEvent == ProgressEvent.Time) {
                yield return new WaitForSeconds(waveEvent.time);
                for (int i = 0; i < instances.Count; i++) {
                    if (instances[i] != null) {
                        if (!constistencies[i]) Destroy(instances[i]);
                    }
                }
            } else if (waveEvent.progressEvent == ProgressEvent.Cleared) {
                while (required.Count != 0) {
                    yield return new WaitForEndOfFrame();
                }
                for (int i = 0; i < instances.Count; i++) {
                    if (instances[i] != null) {
                        if (!constistencies[i]) Destroy(instances[i]);
                    }
                }
                yield return new WaitForSeconds(waveEvent.time);
            }
        }
    }

    GameObject SpawnEntity(SpawnEvent spawnEvent) {
        EventType entityType = spawnEvent.type;
        GameObject entity = null;
        switch (entityType) {
            case EventType.Ram:
                entity = Instantiate(ramPrefab);
                break;
            case EventType.Turret:
                entity = Instantiate(turretPrefab);
                break;
            case EventType.Laser:
                entity = Instantiate(laserPrefab);
                break;
        }
        entity.transform.rotation = SideToRotation(spawnEvent.side);
        entity.transform.position = SpawnPosition(spawnEvent, entity);
        return entity;
    }

    Vector3 SpawnPosition(SpawnEvent spawnEvent, GameObject entity) {
        Vector3 position = Vector3.zero;
        float halfHeight = entity.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
        Side side = spawnEvent.side;
        float offset;
        if (spawnEvent.location == Location.Preset) { offset = spawnEvent.presetLocation; } else { offset = side == Side.Up || side == Side.Down ? Random.Range(-boundsX, boundsX) : Random.Range(-boundsY, boundsY); }
        switch (side) {
            case Side.Up:
                position = new Vector2(offset, boundsY + halfHeight);
                break;
            case Side.Down:
                position = new Vector2(offset, -boundsY - halfHeight);
                break;
            case Side.Right:
                position = new Vector2(boundsX + halfHeight, offset);
                break;
            case Side.Left:
                position = new Vector2(-boundsX - halfHeight, offset);
                break;
        }
        if (spawnEvent.type != EventType.Ram) {
            LerpingEntity lerpingEntity;
            lerpingEntity.entity = entity;
            lerpingEntity.startPos = position;
            lerpingEntity.goalPos = position + (entity.transform.up.normalized * halfHeight * 2);
            lerpingEntity.timePassed = 0;
            lerpingEntities.Add(lerpingEntity);
        }
        return position;
    }

    Quaternion SideToRotation(Side side) {
        switch (side) {
            case Side.Up:
                return Quaternion.Euler(0, 0, 180);
            case Side.Down:
                return Quaternion.Euler(0, 0, 0);
            case Side.Right:
                return Quaternion.Euler(0, 0, 90);
            case Side.Left:
                return Quaternion.Euler(0, 0, 270);
        }
        return Quaternion.identity;
    }
}

[System.Serializable]
public struct WaveEvent {
    public List<SpawnEvent> spawnEvents;
    public ProgressEvent progressEvent;
    public float time;
}

[System.Serializable]
public struct SpawnEvent {
    public EventType type;
    public Side side;
    public Location location;
    public float presetLocation;

    public float delay;
    public bool waitForInitAnimation;

    public Loop loop;

    public bool consistent;
    public bool required;
}

public enum EventType {
    Ram,
    Turret,
    Laser,
    Loop
}

public enum Side {
    Up,
    Down,
    Left,
    Right
}

public enum Location {
    Random,
    Preset
}

public enum ProgressEvent {
    Cleared,
    Time
}

[System.Serializable]
public struct Loop {
    public int start;
    public int end;
    public int amount;
}

[System.Serializable]
public struct LerpingEntity {
    public GameObject entity;
    public Vector2 startPos;
    public Vector2 goalPos;
    public float timePassed;

    public LerpingEntity(GameObject entity, Vector2 startPos, Vector2 goalPos, float timePassed) {
        this.entity = entity;
        this.startPos = startPos;
        this.goalPos = goalPos;
        this.timePassed = timePassed;
    }
}