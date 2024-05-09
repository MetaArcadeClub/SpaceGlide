using System;
using System.Collections;
using System.Threading.Tasks;
using Nakama.CustomYield;
using UnityEngine;
using nk = Nakama.Constants.NakamaConstants;

namespace Nakama.Auth
{
    [CreateAssetMenu(menuName = MenuName, fileName = FilePath + FileName)]
    public class NakamaAuthController : BaseSocialController, INakamaAuth, INakamaAuthResultHandler
    {
        #region consts
        private const string MenuName = "Nakama/Auth/NakamaAuthSO";
        private const string FilePath = "Assets/Nakama/Auth/ScriptableObject/Resources/";
        private const string FileName = "NakamaAuthController";
        #endregion

        #region private variables
        #region coroutines
        private Coroutine _authenticateCoroutine;
        private Coroutine _connectToServerCoroutine;
        #endregion
        #endregion

        #region explicit auth implementations
        void INakamaAuth.ConnectToServer()
        {
            StartConnectToServer();
        }

        void INakamaAuth.AuthenticateDevice()
        {
            StartAuthenticateCoroutine();
        }
        #endregion

        #region explicit auth result implementations
        void INakamaAuthResultHandler.OnConnectedToServer()
        {
            
        }

        void INakamaAuthResultHandler.OnAuthenticatedDevice()
        {
            
        }
        #endregion

        #region private methods
        private string GetDeviceId()
        {
            const string devicePref = nk.DeviceIdentifierPref;
            var deviceId = "";
            if (PlayerPrefs.HasKey(devicePref))
            {
                deviceId = PlayerPrefs.GetString(devicePref);
                return deviceId;
            }
            
            deviceId = SystemInfo.deviceUniqueIdentifier;
            if (deviceId == SystemInfo.unsupportedIdentifier)
                deviceId = Guid.NewGuid().ToString();
            
            PlayerPrefs.SetString(devicePref, deviceId);
            Debug.LogError($"Device Id is :{deviceId}");
            return deviceId;
        }
        #endregion

        #region coroutines
        private void StartAuthenticateCoroutine()
        {
            if (_authenticateCoroutine != null)
            {
                _Mono.StopCoroutine(_authenticateCoroutine);
                _authenticateCoroutine = null;
            }

            var deviceId = GetDeviceId();
            _authenticateCoroutine = _Mono.StartCoroutine(AuthenticateDeviceCoroutine(deviceId));
        }
        private IEnumerator AuthenticateDeviceCoroutine(string deviceId)
        {
            var authTask = AuthenticateDevice(deviceId);
            Debug.LogError($"Authenticating device with ID: {deviceId}");
            yield return new WaitForTask(authTask);
            Debug.LogError($"Authentication Status: {authTask.Status}");

            if (authTask.IsFaulted)
            {
                Debug.LogError($"Authentication failed: {authTask.Exception}");
            }
            else if (authTask.IsCompletedSuccessfully)
            {
                Debug.LogError($"Authentication Completed: {authTask.Id}");
                _Session = authTask.Result;
                StartConnectToServer();
            }
        }

        private void StartConnectToServer()
        {
            if (_connectToServerCoroutine != null)
            {
                _Mono.StopCoroutine(_connectToServerCoroutine);
                _connectToServerCoroutine = null;
            }

            _connectToServerCoroutine = _Mono.StartCoroutine(ConnectToServerCoroutine());
        }

        private IEnumerator ConnectToServerCoroutine()
        {
            var connectTask = ConnectToServer();
            yield return new WaitForTask(connectTask);
            
            Debug.LogError($"Connection Status: {connectTask.Status}");

            if (connectTask.IsFaulted)
            {
                Debug.LogError($"Connection failed: {connectTask.Exception}");
            }
            else if (connectTask.IsCompletedSuccessfully)
            {
                Debug.LogError($"Successfully connected to nakama: {connectTask.Id}");
            }
        }
        #endregion

        #region async methods
        private Task<ISession> AuthenticateDevice(string deviceId)
        {
            return _Client.AuthenticateDeviceAsync(deviceId);
        }

        private Task ConnectToServer()
        {
            return _Socket.ConnectAsync(_Session, true);
        }
        #endregion
    }
}