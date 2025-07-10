using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle; //�þ߰�(120��)
    [SerializeField] private float viewDistance; //�þ߰Ÿ�(10����)
    [SerializeField] private LayerMask targetMask; //Ÿ�ٸ���ũ(�÷��̾�)

    //�ʿ��� ������Ʈ
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
            {//�þ� ���� �÷��̾ �ִٸ�

                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                //������ ����~ �÷��̾ ���� ������ ����
                float angle = Vector3.Angle(_direction, transform.forward);

                if(angle < viewAngle * 0.5)
                {//�þ� ���� �ޱ��� �ִٸ�
                    RaycastHit _hit;
                    if(Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistance))
                    {
                        if(_hit.transform.name == "Player")
                        {
                            Debug.Log("�÷��̾ ������ �þ� ���� ���� !");
                            thePig.Run(_hit.transform.position);
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                        }
                    }
                }
            }
        }
    }
}
