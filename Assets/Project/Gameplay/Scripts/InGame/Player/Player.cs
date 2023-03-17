using GameSource.Utils;
using UnityEngine;

namespace GameSource.InGame.Player
{
    public class Player : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float jumpIntensity;

        [Header("Player Components")]
        [SerializeField] private Rigidbody rb;
        private Vector3 initialPosition;

        private bool gamePaused = true;

        private void Awake()
        {
            EventManager.Subscribe("OnGamePaused", PausePlayer);
            EventManager.Subscribe("OnResetGame", ResetPosition);
        }

        private void Start() { initialPosition = transform.position; }

        private void Update()
        {
            if (gamePaused) return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                rb.velocity = Vector3.up * jumpIntensity;

            if (rb.velocity.y > 0)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-130f, transform.rotation.y, transform.rotation.z), Time.deltaTime * jumpIntensity);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-30f, transform.rotation.y, transform.rotation.z), Time.deltaTime * jumpIntensity);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (gamePaused) return;

            if (other.gameObject.layer == 7)
            {
                EventManager.Trigger("OnGamePaused", true);
                EventManager.Trigger("OnGameOver");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (gamePaused) return;

            if (other.gameObject.layer == 8) EventManager.Trigger("OnPipePass");
        }

        private void PausePlayer(params object[] parameters) { gamePaused = (bool)parameters[0]; }

        private void ResetPosition(params object[] parameters)
        {
            rb.velocity = Vector3.zero;
            transform.position = initialPosition;
        }
    }
}

