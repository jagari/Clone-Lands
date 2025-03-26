using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
    [SerializeField] GameObject[] enemyLinUp = new GameObject[6];
    List<List<GameObject>> fleetList;
    int numColumn = 7;
    int numActiveEnemys = 0;
    int totalEnemys = 0;


    float enemyMoveTimer = 1;
    float stepDelay = 2;

    float hStep = 0.1f;
    float vStep = 0.05f;

    const float rightLimit = 4.5f;
    const float leftLimit = -4.5f;

    float leftMostPosition;
    float rightMostPosition;
    float bottomMostPosition;

    MoveState moveState;

    private enum MoveState {
        None = 0,
        Left,
        Right, 
        Down
    }

     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        SpawnFleet();
        moveState = MoveState.None;
    }

    // Update is called once per frame
    void Update() {
        if (IsEnemyMoveTime())
            return;

        UpdateParameters();

        UpdateFleetMovement();
    }

    private void SpawnFleet() {
        fleetList = new List<List<GameObject>>();

        totalEnemys = 0;

        for (int i = 0; i < numColumn; i ++) {
            List<GameObject> column = new List<GameObject>();

            for (int j = 0; j < enemyLinUp.Length; j++) {
                GameObject enemy = Instantiate(enemyLinUp[j]);
                column.Add(enemy);
                PositionEnemy(enemy, i, j);
                totalEnemys++;
            }

            fleetList.Add(column);
        }
    }

    private void PositionEnemy(GameObject enemy, int column, int row) {
        float offsetX = 1.2f;
        float offsetY = 1;

        float startX = transform.position.x + 0.6f + (-offsetX * (float)numColumn / 2);
        float startY = transform.position.y + (offsetY * 18);

        float posX = startX + (float)column * offsetX;
        float posY = startY -  (float)row * offsetY;

        enemy.transform.position = new Vector2(posX, posY);
    }

    private bool IsEnemyMoveTime() {
        bool result = false;

        enemyMoveTimer -= Time.deltaTime;
        if (enemyMoveTimer > 0) {
            result = true;
        }
        return result;
    }

    private void MoveFleet(Vector2 direction, float distance) {
        for (int i = 0; i < fleetList.Count; i++){
            List<GameObject> column = fleetList[i];

            for (int j = 0; j < column.Count; j++){
                GameObject enemy = column[j];

                if(!enemy.activeInHierarchy) {
                    continue;
                }

                enemy.GetComponent<Enemy>().EnemyMove(direction, distance);
            }
        }

        EnemyStepDelay();
    }

    private void EnemyStepDelay() {
        int activeEnemys = numActiveEnemys;

        if (activeEnemys < 2) {
            activeEnemys = 2;
        }

        enemyMoveTimer = stepDelay * ((float)numActiveEnemys / totalEnemys);
    }

    private void UpdateParameters() {
       numActiveEnemys = 0;

        leftMostPosition = float.MaxValue;
        rightMostPosition = -float.MaxValue;
        bottomMostPosition = float.MaxValue;

        for (int i = 0; i < fleetList.Count; i++) {
            List<GameObject> column = fleetList[i];

            for (int j = 0; j < column.Count; j++) {
                GameObject enemy = column[j];

                if (enemy.activeInHierarchy) {
                    numActiveEnemys++;

                    Vector2 enemyPosition = enemy.transform.position;

                    if (enemyPosition.x > rightMostPosition) {
                        rightMostPosition = enemyPosition.x;
                    }

                    if (enemyPosition.x < leftMostPosition) {
                        leftMostPosition = enemyPosition.x;
                    }

                    if (enemyPosition.y < bottomMostPosition) {
                        bottomMostPosition = enemyPosition.y;
                    }
            }
        }
    }
}

    private void UpdateFleetMovement() {
        switch (moveState) {
            case MoveState.None:
                MoveStateEntrance();
                break;

            case MoveState.Left:
                MoveStateLeft();
                break;

            case MoveState.Right:
                MoveStateRight();
                break;

            case MoveState.Down:
                MoveStateDown();
                break;

        }
    }

    private void MoveStateEntrance() {
        MoveFleet(Vector2.down, hStep * 7);
        
        EnterMoveStateLeft();

    }

    private void MoveStateLeft() {
        if (leftMostPosition <= leftLimit) {
            EnterMoveStateDown();
        }
        else{
            MoveFleet(Vector2.left, SpeedVariance());
        }
    }

    private void MoveStateRight() {
        if (rightMostPosition >= rightLimit) {
            EnterMoveStateDown();
        }
        else{
            MoveFleet(Vector2.right, SpeedVariance());
        }
    }

    private void MoveStateDown() {
        MoveFleet(Vector2.down, hStep);

        if (rightMostPosition >= rightLimit) {
            EnterMoveStateLeft();
        }
        else{
            EnterMoveStateRight();
        }
    }

    private void EnterMoveStateLeft() => moveState = MoveState.Left;

    private void EnterMoveStateRight() => moveState = MoveState.Right;

    private void EnterMoveStateDown() => moveState = MoveState.Down;

    private float SpeedVariance() {
        float val = 0.02f;

        return Random.Range(vStep - val, vStep + val);
    }
}
