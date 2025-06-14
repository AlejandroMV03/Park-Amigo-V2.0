package com.example.parkamigo.ui

import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.ui.text.font.FontWeight

@Composable
fun PantallaConBackSimple(titulo: String, onBack: () -> Unit) {
    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp)
    ) {
        Button(onClick = onBack) {
            Text("Regresar")
        }
        Text("Sección: $titulo", fontSize = 20.sp, fontWeight = FontWeight.Bold)

        when (titulo) {
            "Usuarios" -> UusuariosScreen()
            "Reservas" -> Text("Aquí van las reservas")
            "Estacionamiento" -> Text("Aquí va el estacionamiento")
            "Tarjetas" -> Text("Aquí van las tarjetas")
        }
    }
}
