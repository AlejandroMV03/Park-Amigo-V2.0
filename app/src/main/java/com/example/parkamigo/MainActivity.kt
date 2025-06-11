package com.example.parkamigo

import android.os.Bundle
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.parkamigo.ui.AdminHomeScreen
import com.example.parkamigo.ui.theme.ParkAmigoTheme
import com.google.firebase.firestore.FirebaseFirestore
import com.example.parkamigo.ui.UserHomeScreen


class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            ParkAmigoTheme {
                var currentUserType by remember { mutableStateOf<String?>(null) }

                when (currentUserType) {
                    null -> LoginScreen { userType ->
                        currentUserType = userType
                    }
                    "admin" -> AdminHomeScreen(
                        onLogout = { currentUserType = null }
                    )
                    "user" -> UserHomeScreen(
                        onLogout = { currentUserType = null }
                    )
                }
            }
        }
    }
}

@Composable
fun LoginScreen(onLoginSuccess: (String) -> Unit) {
    val context = LocalContext.current

    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var isLoading by remember { mutableStateOf(false) }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(24.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Image(
            painter = painterResource(id = R.drawable.pka),
            contentDescription = "Logo de Park Amigo",
            modifier = Modifier
                .size(120.dp)
                .padding(bottom = 16.dp)
        )

        Text(
            text = "Park Amigo",
            fontSize = 32.sp,
            fontWeight = FontWeight.Bold
        )

        Spacer(modifier = Modifier.height(32.dp))

        OutlinedTextField(
            value = username,
            onValueChange = { username = it },
            label = { Text("Nombre de usuario o tarjeta") },
            singleLine = true
        )

        Spacer(modifier = Modifier.height(16.dp))

        OutlinedTextField(
            value = password,
            onValueChange = { password = it },
            label = { Text("Contraseña") },
            singleLine = true,
            visualTransformation = PasswordVisualTransformation(),
            keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password)
        )

        Spacer(modifier = Modifier.height(24.dp))

        Button(
            onClick = {
                isLoading = true
                loginUser(username.trim(), password.trim()) { success, message, userType ->
                    isLoading = false
                    Toast.makeText(context, message, Toast.LENGTH_LONG).show()
                    if (success && userType != null) {
                        onLoginSuccess(userType)
                    }
                }
            },
            enabled = !isLoading
        ) {
            if (isLoading) {
                CircularProgressIndicator(
                    modifier = Modifier.size(24.dp),
                    color = MaterialTheme.colorScheme.onPrimary,
                    strokeWidth = 2.dp
                )
            } else {
                Text("Iniciar sesión")
            }
        }
    }
}

fun loginUser(
    usernameInput: String,
    passwordInput: String,
    onResult: (Boolean, String, String?) -> Unit
) {
    val db = FirebaseFirestore.getInstance()
    val adminCollection = db.collection("Administrador")
    val userCollection = db.collection("Registro-Usuario")

    adminCollection.whereEqualTo("Tarjeta", usernameInput)
        .get()
        .addOnSuccessListener { tarjetaResult ->
            if (!tarjetaResult.isEmpty) {
                onResult(true, "Bienvenido administrador por tarjeta", "admin")
            } else {
                adminCollection.whereEqualTo("NombreDeAdministrador", usernameInput)
                    .get()
                    .addOnSuccessListener { nombreResult ->
                        if (!nombreResult.isEmpty) {
                            val adminDoc = nombreResult.documents[0]
                            val passReal = adminDoc.getString("Contraseña")
                            if (passReal == passwordInput) {
                                onResult(true, "Bienvenido administrador", "admin")
                            } else {
                                onResult(false, "Contraseña incorrecta para administrador", null)
                            }
                        } else {
                            userCollection.whereEqualTo("Nombre_de_usuario", usernameInput)
                                .get()
                                .addOnSuccessListener { userResult ->
                                    if (!userResult.isEmpty) {
                                        val userDoc = userResult.documents[0]
                                        val passReal = userDoc.getString("Contraseña")
                                        if (passReal == passwordInput) {
                                            onResult(true, "Bienvenido usuario registrado", "user")
                                        } else {
                                            onResult(false, "Contraseña incorrecta para usuario", null)
                                        }
                                    } else {
                                        onResult(false, "Usuario no encontrado", null)
                                    }
                                }
                                .addOnFailureListener { e ->
                                    onResult(false, "Error en Registro-Usuario: ${e.message}", null)
                                }
                        }
                    }
                    .addOnFailureListener { e ->
                        onResult(false, "Error buscando NombreDeAdministrador: ${e.message}", null)
                    }
            }
        }
        .addOnFailureListener { e ->
            onResult(false, "Error buscando tarjeta: ${e.message}", null)
        }
}

//@Composable
//fun UserHomeScreen(onLogout: () -> Unit) {
   // Column(
        //modifier = Modifier.fillMaxSize(),
       // horizontalAlignment = Alignment.CenterHorizontally,
        //verticalArrangement = Arrangement.Center
   // ) {
       // Text("Pantalla de Usuario", fontWeight = FontWeight.Bold, fontSize = 24.sp)
       // Spacer(modifier = Modifier.height(16.dp))
       // Button(onClick = onLogout) {
         //   Text("Cerrar sesión")
       // }
    //}
//}

@Preview(showBackground = true)
@Composable
fun LoginScreenPreview() {
    ParkAmigoTheme {
        LoginScreen {}
    }
}
