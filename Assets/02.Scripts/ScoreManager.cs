using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
   
    // 관리 : 특정 데이터에 대한 무결성과 생성 읽기 삭제 수정 등과 관련된 로직을 
    private int _bestscore;
    private int _currentScore;
    
    
    // UI 책임 추가 
    [SerializeField] private TextMeshProUGUI _bestScoreTextUI;
    [SerializeField] private TextMeshProUGUI _currentScoreTextUI;

    private void Update()
    {
        _bestScoreTextUI.text = $"BestScore: {_bestscore}";
        _currentScoreTextUI.text = $"Score: {_currentScore}";
    }


}
