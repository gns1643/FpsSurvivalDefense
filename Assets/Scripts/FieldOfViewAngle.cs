using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle; //시야각(120도)
    [SerializeField] private float viewDistance; //시야거리(10미터)
    [SerializeField] private LayerMask targetMask; //타겟마스크(플레이어)

    //필요한 컴포넌트
    private Pig thePig;
    private void Start()
    {
        thePig = GetComponent<Pig>();
    }
    private void Update()
    {
        View();
    }
    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {//시야 내에 플레이어가 있다면

                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                //돼지의 정면~ 플레이어를 향한 벡터의 각도
                float angle = Vector3.Angle(_direction, transform.forward);

                if(angle < viewAngle * 0.5)
                {//시야 내에 앵글이 있다면
                    RaycastHit _hit;
                    if(Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if(_hit.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 돼지의 시야 내에 있음 !");
                            thePig.Run(_hit.transform.position);
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                        }
                    }
                }
            }
        }
    }
}
