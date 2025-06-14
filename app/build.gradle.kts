plugins {
    alias(libs.plugins.android.application)
    alias(libs.plugins.kotlin.android)
    alias(libs.plugins.kotlin.compose)

    id("com.google.gms.google-services")
    id("kotlin-kapt")
}

android {
    namespace = "com.example.parkamigo"
    compileSdk = 35

    defaultConfig {
        applicationId = "com.example.parkamigo"
        minSdk = 28
        targetSdk = 35
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_11
        targetCompatibility = JavaVersion.VERSION_11
    }

    kotlinOptions {
        jvmTarget = "11"
    }

    buildFeatures {
        compose = true
    }
}

dependencies {
    implementation(libs.androidx.core.ktx)
    implementation(libs.androidx.lifecycle.runtime.ktx)
    implementation(libs.androidx.activity.compose)
    implementation(platform(libs.androidx.compose.bom))
    implementation(libs.androidx.ui)
    implementation(libs.androidx.ui.graphics)
    implementation(libs.androidx.ui.tooling.preview)
    implementation(libs.androidx.material3)

    // Firebase
    implementation(platform("com.google.firebase:firebase-bom:33.15.0"))
    implementation("com.google.firebase:firebase-firestore-ktx")

    // Coil para GIFs
    implementation("io.coil-kt:coil-compose:2.4.0")
    implementation("io.coil-kt:coil-gif:2.4.0")

    // Glide
    implementation("com.github.bumptech.glide:glide:4.15.1")

    // Foundation para LazyVerticalGrid
    implementation("androidx.compose.foundation:foundation:1.5.0")

    // Compatibilidad
    implementation("androidx.appcompat:appcompat:1.6.1")

    implementation ("androidx.compose.material:material-icons-extended:1.4.3")

    // Tests
    testImplementation(libs.junit)
    androidTestImplementation(libs.androidx.junit)
    androidTestImplementation(libs.androidx.espresso.core)
    androidTestImplementation(platform(libs.androidx.compose.bom))
    androidTestImplementation(libs.androidx.ui.test.junit4)
    debugImplementation(libs.androidx.ui.tooling)
    debugImplementation(libs.androidx.ui.test.manifest)
}
