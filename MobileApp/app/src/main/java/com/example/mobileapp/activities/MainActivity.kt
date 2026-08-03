package com.example.mobileapp.activities

import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.app.AppCompatDelegate
import androidx.core.content.edit
import androidx.fragment.app.Fragment
import com.example.mobile1.fragments.PhotoFragment
import com.example.mobileapp.R
import com.example.mobileapp.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {

    lateinit var binding: ActivityMainBinding
    override fun onCreate(savedInstanceState: Bundle?) {
        defineThemeAndApply()
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)
        with(binding) {
            bottomNav.setOnItemSelectedListener { item ->
                when (item.itemId) {
                    R.id.photo -> {
                        setFragment(PhotoFragment())
                    }
                    R.id.note -> {
                        setFragment(PhotoFragment())
                    }
                    R.id.main -> {
                        setFragment(PhotoFragment())
                    }
                    R.id.video -> {
                        setFragment(PhotoFragment())
                    }
                }
                true
            }
        }

    }
    fun changeThemeAndApply () {
        getSharedPreferences("settings", MODE_PRIVATE).apply {
            var theme = getString("theme", "light")
            edit(commit = true) {
                if (theme == "light") {
                    putString("theme", "dark")
                    AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_YES)
                } else {
                    putString("theme", "light")
                    AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_NO)
                }
            }
        }
    }

    fun defineThemeAndApply() {
        getSharedPreferences("settings", MODE_PRIVATE).apply {
            var theme = getString("theme", "light")
            if (theme == "light") {
                AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_NO)
            }
            else {
                AppCompatDelegate.setDefaultNightMode(AppCompatDelegate.MODE_NIGHT_YES)
            }
        }
    }

    fun setFragment(f: Fragment) = supportFragmentManager.beginTransaction().apply {
        replace(R.id.mainFrame, f)
        commit()
    }


    override fun onCreateOptionsMenu(menu: Menu?): Boolean {
        menuInflater.inflate(R.menu.upper_menu,menu)
        return super.onCreateOptionsMenu(menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        when (item.itemId) {
            R.id.theme -> {
                changeThemeAndApply()
            }
        }
        return true
    }
}