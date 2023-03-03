using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Transform[] slots;
    public ScoreProperties[] allScores;
    [SerializeField] ExtraScorePaterns[] specialPaterns;
    [SerializeField] Text scoreText;
    [SerializeField] Text scoreTextAnim;
    private int myScore;
    private int pointsToAdd;
    private bool isThereBasicScore, isThereExtraScore;
    [SerializeField] Button spinBtn;
    [SerializeField] AudioManager audioManager;

    [SerializeField] private int[,] figuresGrid = new int[3, 5]
    {
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 }
    };

    #region Chuleta de valores de las figuras
    /* Figures values:
    Bell = 0
    Cherry = 1
    Grape = 2
    Lemon = 3
    Olive = 4
    Orange = 5
    Watermelon = 6
    */
    #endregion

    public void ChangeScoreGrid() //Cambiar array bidimensional con el que determinaremos si se ha ganado algo
    {
        int[,] scoreGridClone = figuresGrid;
        for (int column = 0; column < 5; column++)
        {
            for (int row = 0; row < 3; row++)
                scoreGridClone[row, column] = slots[column].transform.GetChild(0).transform.GetChild(row).GetComponent<FigureValue>().value;
        }
        StartCoroutine("CheckScoresInOrder", scoreGridClone);
    }

    IEnumerator CheckScoresInOrder(int[,] scoreGrid)
    {
        CheckBasicScorePaterns();
        if(isThereBasicScore)
            yield return new WaitForSeconds(1.3f);
        CheckExtraScorePaterns(scoreGrid);
        if(isThereExtraScore)
            yield return new WaitForSeconds(1.3f);
        spinBtn.interactable = true;
        isThereBasicScore = false;
        isThereExtraScore = false;
    }

    private void CheckBasicScorePaterns() //Determinar si hay algún patrón básico de puntos
    {
        List<GameObject> winningFigures = new List<GameObject>();

        for (int row = 0; row < 3; row++)
        {
            int firstRowFigureValue = slots[0].transform.GetChild(0).transform.GetChild(row).GetComponent<FigureValue>().value;
            int repetitions = 1;
            winningFigures.Add(slots[0].transform.GetChild(0).transform.GetChild(row).gameObject);

            for (int column = 1; column < 5; column++)
            {
                int nextRowFigureValue = slots[column].transform.GetChild(0).transform.GetChild(row).GetComponent<FigureValue>().value;
                if (nextRowFigureValue == firstRowFigureValue)
                {
                    repetitions++;
                    winningFigures.Add(slots[column].transform.GetChild(0).transform.GetChild(row).gameObject);
                }
                else if (nextRowFigureValue != firstRowFigureValue)
                    break;
            }

            if (repetitions > 2){
                isThereBasicScore = true;
                spinBtn.interactable = false;
                GetScore(firstRowFigureValue, repetitions, false);
                ShowWinningFigures(winningFigures);
            }
            else{
                winningFigures.Clear();
            }
        }
    }

    private void CheckExtraScorePaterns(int[,] _scoreArray) //Determinar si hay algún patrón extra de puntos
    {
        List<GameObject> winningFigures = new List<GameObject>();

        for (int patern = 0; patern < specialPaterns.Length; patern++)
        {
            int firstValue = 0;
            int repetitions = 1;
            bool isFirstValue = true;
            for (int coordinate = 0; coordinate < specialPaterns[patern].coordinatesList.Count; coordinate++)
            {
                if (!isFirstValue){
                    int nextValue = _scoreArray[(int)specialPaterns[patern].coordinatesList[coordinate].x,
                        (int)specialPaterns[patern].coordinatesList[coordinate].y];
                    if (firstValue == nextValue)
                        repetitions++;
                    else
                        break;
                }
                else{
                    firstValue = _scoreArray[(int)specialPaterns[patern].coordinatesList[coordinate].x,
                        (int)specialPaterns[patern].coordinatesList[coordinate].y];
                    isFirstValue = false;
                }
                winningFigures.Add(slots[(int)specialPaterns[patern].coordinatesList[coordinate].y].transform.GetChild(0)
                        .transform.GetChild((int)specialPaterns[patern].coordinatesList[coordinate].x).gameObject);
            }

            if (repetitions >= 3){
                isThereExtraScore = true;
                GetScore(firstValue, repetitions, true);
                ShowWinningFigures(winningFigures);
            }
            else{
                winningFigures.Clear();
            }
        }
    }

    private void ShowWinningFigures(List<GameObject> _winningFigures) //Activar la animación de cada figura que este en un patron de ganar puntos
    {
        foreach (var figure in _winningFigures)
        {
            figure.GetComponent<Animator>().SetTrigger("Win");
        }
        _winningFigures.Clear();
    }

    private void GetScore(int _figureValue, int _repetitions, bool _isSpecial) //Acceder a los puntuajes y sumar el que pertoque
    {
        for (int allFiguresScores = 0; allFiguresScores < allScores.Length; allFiguresScores++)
        {
            if (allScores[allFiguresScores].figureValue == _figureValue)
            {
                if (!_isSpecial)
                {
                    if (_repetitions == 3){
                        myScore += allScores[allFiguresScores].threeScore;
                        pointsToAdd += allScores[allFiguresScores].threeScore;
                    }
                    else if (_repetitions == 4){
                        myScore += allScores[allFiguresScores].fourScore;
                        pointsToAdd += allScores[allFiguresScores].fourScore;
                    }
                    else if (_repetitions == 5){
                        myScore += allScores[allFiguresScores].fiveScore;
                        pointsToAdd += allScores[allFiguresScores].fiveScore;
                    }
                }
                if(_isSpecial)
                {
                    if (_repetitions == 3){
                        myScore += allScores[allFiguresScores].specialScore3;
                        pointsToAdd += allScores[allFiguresScores].specialScore3;
                    }
                    if (_repetitions == 4){
                        myScore += allScores[allFiguresScores].specialScore4;
                        pointsToAdd += allScores[allFiguresScores].specialScore4;
                    }
                    if (_repetitions == 5){
                        myScore += allScores[allFiguresScores].specialScore5;
                        pointsToAdd += allScores[allFiguresScores].specialScore5;
                    }
                }
            }
            StartCoroutine("AddPoints");
        }
    }

    IEnumerator AddPoints() //Añadir puntos visual
    {
        audioManager.PlaySound("PlusPoints1");
        yield return new WaitForSeconds(0.2f);
        scoreTextAnim.text = pointsToAdd.ToString();
        scoreTextAnim.GetComponent<Animator>().SetTrigger("Gain");
        yield return new WaitForSeconds(1f);
        audioManager.PlaySound("Coin1");
        scoreText.text = myScore.ToString();
        pointsToAdd = 0;
    }
}

[System.Serializable]
public class ScoreProperties
{
    public string type;
    public int figureValue;
    public int threeScore;
    public int fourScore;
    public int fiveScore;
    public int specialScore3;
    public int specialScore4;
    public int specialScore5;
}

[System.Serializable]
public class ExtraScorePaterns
{
    public List<Vector2> coordinatesList;
}