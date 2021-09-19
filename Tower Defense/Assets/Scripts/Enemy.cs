using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private SpriteRenderer _healthBar;
    [SerializeField] private SpriteRenderer _healthFill;

    private float _speedBeforeSlow;
    private bool _isSlow;
    private SpriteRenderer _sp;
    public float _slowTime;

    private int _currentHealth;

    public Vector3 TargetPosition { get; private set; }
    public int CurrentPathIndex { get; private set; }

    private void Update() {
        if(_isSlow)
            _slowTime -= Time.deltaTime;
            _sp.color = Color.blue;

        if(_slowTime <= 0){
            _isSlow = false;
            _moveSpeed = _speedBeforeSlow;
            _slowTime = 0;
            _sp.color = Color.white;
        }
    }

    // Fungsi ini terpanggil sekali setiap kali menghidupkan game object yang memiliki script ini
    private void OnEnable ()
    {
        _currentHealth = _maxHealth;
        _healthFill.size = _healthBar.size;
        _speedBeforeSlow = _moveSpeed;
        _isSlow = false;
        _slowTime = 0;
        _sp = this.GetComponent<SpriteRenderer>();
    }

    public void MoveToTarget ()
    {
        transform.position = Vector3.MoveTowards (transform.position, TargetPosition, _moveSpeed * Time.deltaTime);
    }

    public void SetTargetPosition (Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        _healthBar.transform.parent = null;

        // Mengubah rotasi dari enemy
        Vector3 distance = TargetPosition - transform.position;

        if (Mathf.Abs (distance.y) > Mathf.Abs (distance.x))
        {
            // Menghadap atas
            if (distance.y > 0)
            {
                transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 90f));
            }
            // Menghadap bawah
            else
            {
                transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -90f));
            }
        }
        else
        {
            // Menghadap kanan (default)
            if (distance.x > 0)
            {
                transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
            }

            // Menghadap kiri
            else
            {
                transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 180f));
            }
        }
        _healthBar.transform.parent = transform;

    }

    public void ReduceEnemyHealth (int damage)
    {
        _currentHealth -= damage;
        AudioPlayer.Instance.PlaySFX ("hit-enemy");

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            gameObject.SetActive (false);
            AudioPlayer.Instance.PlaySFX ("enemy-die");
        }

        float healthPercentage = (float) _currentHealth / _maxHealth;
        _healthFill.size = new Vector2 (healthPercentage * _healthBar.size.x, _healthBar.size.y);
    }

    public void ReduceEnemyHealth(int damage, float slow){
        ReduceEnemyHealth(damage);
        if(!_isSlow){
            if(_moveSpeed == 1)
                _moveSpeed /= 2;
            else if(_moveSpeed > 1)
                _moveSpeed -= slow;
        }
        _slowTime += 3f;
        _isSlow = true;
    }

    // Menandai indeks terakhir pada path
    public void SetCurrentPathIndex (int currentIndex)
    {
        CurrentPathIndex = currentIndex;
    }
}
