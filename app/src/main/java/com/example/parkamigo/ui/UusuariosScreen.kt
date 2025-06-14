package com.example.parkamigo.ui

import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.google.firebase.firestore.FirebaseFirestore
import androidx.compose.ui.text.font.FontWeight

@Composable
fun UusuariosScreen() {
    var listaUsuarios by remember { mutableStateOf(listOf<Usuario>()) }
    var busqueda by remember { mutableStateOf("") }

    LaunchedEffect(Unit) {
        val db = FirebaseFirestore.getInstance()
        db.collection("Registro-Usuario").get().addOnSuccessListener { registros ->
            val usuariosTemp = mutableListOf<Usuario>()
            for (registro in registros) {
                val id = registro.id
                val nombre = registro.getString("Nombre") ?: ""
                val apellido = registro.getString("Apellido") ?: ""
                val nombreUsuario = registro.getString("Nombre_de_usuario") ?: ""
                val celular = registro.getString("Numero_de_celular") ?: ""
                val contrasena = registro.getString("Contraseña") ?: ""

                db.collection("Tarjetas")
                    .whereEqualTo("FK_Registro", id.toIntOrNull())
                    .get()
                    .addOnSuccessListener { tarjetas ->
                        val numeroTarjeta = tarjetas.documents.firstOrNull()?.getString("Numero_Tarjeta")
                        usuariosTemp.add(
                            Usuario(
                                nombre,
                                apellido,
                                nombreUsuario,
                                celular,
                                contrasena,
                                numeroTarjeta
                            )
                        )
                        listaUsuarios = usuariosTemp.toList()
                    }
            }
        }
    }

    val filtrados = listaUsuarios.filter {
        it.nombreUsuario.contains(busqueda, ignoreCase = true)
    }

    Column(modifier = Modifier.padding(16.dp)) {
        OutlinedTextField(
            value = busqueda,
            onValueChange = { busqueda = it },
            label = { Text("Buscar por nombre de usuario") },
            modifier = Modifier.fillMaxWidth()
        )

        Spacer(modifier = Modifier.height(16.dp))

        Column {
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                Text("Nombre", fontWeight = FontWeight.Bold)
                Text("Apellido", fontWeight = FontWeight.Bold)
                Text("Usuario", fontWeight = FontWeight.Bold)
                Text("Celular", fontWeight = FontWeight.Bold)
                Text("Contraseña", fontWeight = FontWeight.Bold)
                Text("Tarjeta", fontWeight = FontWeight.Bold)
            }
            Divider()
            filtrados.forEach { usuario ->
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 4.dp),
                    horizontalArrangement = Arrangement.SpaceBetween
                ) {
                    Text(usuario.nombre)
                    Text(usuario.apellido)
                    Text(usuario.nombreUsuario)
                    Text(usuario.celular)
                    Text(usuario.contrasena)
                    Text(usuario.numeroTarjeta ?: "N/A")
                }
            }
        }
    }
}

data class Usuario(
    val nombre: String,
    val apellido: String,
    val nombreUsuario: String,
    val celular: String,
    val contrasena: String,
    val numeroTarjeta: String?
)
