package com.example.parkamigo

import android.os.Bundle
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.Canvas
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.Offset
import androidx.compose.ui.platform.LocalContext

import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.parkamigo.ui.AdminHomeScreen
import com.example.parkamigo.ui.UserHomeScreen
import com.example.parkamigo.ui.theme.ParkAmigoTheme
import com.google.firebase.firestore.FirebaseFirestore
import coil.compose.AsyncImage
import coil.decode.GifDecoder
import coil.request.ImageRequest
import kotlinx.coroutines.delay
import kotlin.random.Random
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            ParkAmigoTheme {
                var showSplash by remember { mutableStateOf(true) }
                var currentUserType by remember { mutableStateOf<String?>(null) }

                if (showSplash) {
                    SplashScreen {
                        showSplash = false
                    }
                } else {
                    when (currentUserType) {
                        null -> LoginScreen { userType ->
                            currentUserType = userType
                        }
                        "admin" -> AdminHomeScreen(onLogout = { currentUserType = null })
                        "user" -> UserHomeScreen(onLogout = { currentUserType = null })
                    }
                }
            }
        }
    }
}

@Composable
fun SplashScreen(onTimeout: () -> Unit) {
    val context = LocalContext.current

    LaunchedEffect(true) {
        delay(3100)
        onTimeout()
    }

    Box(
        modifier = Modifier.fillMaxSize(),
        contentAlignment = Alignment.Center
    ) {
        AsyncImage(
            model = ImageRequest.Builder(context)
                .data(R.drawable.ftp)
                .decoderFactory(GifDecoder.Factory())
                .build(),
            contentDescription = "Splash animado",
            modifier = Modifier.fillMaxSize(),
            contentScale = ContentScale.Crop
        )
    }
}

@Composable
fun StarryBackground(modifier: Modifier = Modifier) {
    val starCount = 100
    val stars = remember {
        List(starCount) {
            Offset(
                x = Random.nextInt(0, 1000).toFloat(),
                y = Random.nextInt(0, 2000).toFloat()
            )
        }
    }

    val alphaValues = remember { mutableStateListOf<Float>() }

    LaunchedEffect(Unit) {
        repeat(starCount) {
            alphaValues.add(Random.nextFloat() * (1f - 0.2f) + 0.2f)
        }
        while (true) {
            delay(300)
            for (i in alphaValues.indices) {
                alphaValues[i] = Random.nextFloat() * (1f - 0.2f) + 0.2f
            }
        }
    }

    Canvas(modifier = modifier.fillMaxSize()) {
        for (i in stars.indices) {
            drawCircle(
                color = Color.White.copy(alpha = alphaValues.getOrNull(i) ?: 0.5f),
                radius = 1.5f,
                center = stars[i]
            )
        }
    }
}

@Composable
fun LoginScreen(onLoginSuccess: (String) -> Unit) {
    val context = LocalContext.current

    var username by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var isLoading by remember { mutableStateOf(false) }

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(Color.Black)
    ) {
        StarryBackground()

        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(24.dp),
            contentAlignment = Alignment.Center
        ) {
            Column(
                verticalArrangement = Arrangement.Center,
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                AsyncImage(
                    model = ImageRequest.Builder(context)
                        .data(R.drawable.pafk)
                        .decoderFactory(GifDecoder.Factory())
                        .build(),
                    contentDescription = "Carro y Moto animado",
                    modifier = Modifier.size(140.dp)
                )

                Spacer(modifier = Modifier.height(16.dp))

                Text(
                    text = "Park Amigo",
                    fontSize = 32.sp,
                    fontWeight = FontWeight.Bold,
                    color = Color.White
                )

                Spacer(modifier = Modifier.height(32.dp))

                OutlinedTextField(
                    value = username,
                    onValueChange = { username = it },
                    label = { Text("Nombre de usuario o tarjeta", color = Color.White) },
                    singleLine = true,
                    colors = OutlinedTextFieldDefaults.colors(
                        unfocusedContainerColor = Color.Transparent,
                        focusedContainerColor = Color.Transparent,
                        unfocusedLabelColor = Color.White,
                        focusedLabelColor = Color.White,
                        unfocusedTextColor = Color.White,
                        focusedTextColor = Color.White
                    )
                )

                Spacer(modifier = Modifier.height(16.dp))

                OutlinedTextField(
                    value = password,
                    onValueChange = { password = it },
                    label = { Text("Contraseña", color = Color.White) },
                    singleLine = true,
                    visualTransformation = PasswordVisualTransformation(),
                    keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Password),
                    colors = OutlinedTextFieldDefaults.colors(
                        unfocusedContainerColor = Color.Transparent,
                        focusedContainerColor = Color.Transparent,
                        unfocusedLabelColor = Color.White,
                        focusedLabelColor = Color.White,
                        unfocusedTextColor = Color.White,
                        focusedTextColor = Color.White
                    )
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
                            userCollection.whereEqualTo("nombre_de_usuario", usernameInput)
                                .get()
                                .addOnSuccessListener { userResult ->
                                    if (!userResult.isEmpty) {
                                        val userDoc = userResult.documents[0]
                                        val passReal = userDoc.getString("contraseña")
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

@Preview(showBackground = true)
@Composable
fun LoginScreenPreview() {
    ParkAmigoTheme {
        LoginScreen {}
    }
}
