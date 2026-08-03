package com.example.mobileapp.adapters

import android.content.Context
import android.graphics.BitmapFactory
import android.util.Base64
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.mobileapp.R
import com.example.mobileapp.models.Report


class ReportAdapter(var cont: Context, var list: ArrayList<Report>) : RecyclerView.Adapter<ReportAdapter.CardViewHolder>() {
    override fun onCreateViewHolder(
        parent: ViewGroup,
        viewType: Int
    ): CardViewHolder {
        return CardViewHolder(LayoutInflater.from(cont).inflate(R.layout.report_item, parent, false))
    }

    override fun onBindViewHolder(
        holder: CardViewHolder,
        position: Int
    ) {
        holder.bind(list[position])
    }

    override fun getItemCount(): Int {
        return list.size
    }

    class CardViewHolder (view: View): RecyclerView.ViewHolder(view) {
            var titleView = view.findViewById<TextView>(R.id.reportTitleView)
            var textView = view.findViewById<TextView>(R.id.reportTextView)
            var iconView = view.findViewById<TextView>(R.id.reportIconView)
            var reportDateView = view.findViewById<TextView>(R.id.reportDateView)
            var reportImageView = view.findViewById<ImageView>(R.id.reportImageView)

        fun bind(item: Report) {
            titleView.text = item.title
            textView.text = item.text
            reportDateView.text = item.date
             if (item.imageString != null) {
                 var bytes= Base64.decode(item.imageString.toByteArray(), Base64.DEFAULT)
                 var bitm = BitmapFactory.decodeByteArray(bytes, 0, bytes.size)
                 reportImageView.setImageBitmap(bitm);
             }

            var icon = "📷"
            when(item.typeId) {
                2->{icon="📽️"}
                3->{icon="📝"}
            }
            iconView.text = icon
        }
    }

}