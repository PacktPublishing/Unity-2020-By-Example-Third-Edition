package com.unity.oculus;

import android.app.Activity;
import android.view.Surface;
import android.view.SurfaceView;
import android.util.Log;
import android.view.ViewGroup;
import com.unity3d.player.UnityPlayer;

public class OculusUnity
{
    UnityPlayer player;
    Activity activity;
    SurfaceView glView;

    public void initOculus()
    {
        Log.d("Unity", "initOculus Java!");
        activity = UnityPlayer.currentActivity;

        activity.runOnUiThread(() -> {

            ViewGroup vg = activity.findViewById(android.R.id.content);
            player = null;
            for (int i = 0; i < vg.getChildCount(); ++i) {
                if (vg.getChildAt(i) instanceof UnityPlayer) {
                    player = (UnityPlayer) vg.getChildAt(i);
                    break;
                }
            }

            if (player == null) {
                Log.e("Unity", "Failed to find UnityPlayer view!");
                return;
            }

            glView = null;
            for (int i = 0; i < player.getChildCount(); ++i)
            {
                if (player.getChildAt(0) instanceof SurfaceView)
                {
                    glView = (SurfaceView)player.getChildAt(0);
                }
            }

            if (glView == null) {
                Log.e("Unity", "Failed to find GlView!");
            }

            Log.d("Unity", "Oculus UI thread done.");

            initComplete(glView.getHolder().getSurface());
        });
    }

    public void pauseOculus()
    {

    }

    public void resumeOculus()
    {

    }

    public void destroyOculus()
    {

    }


    private native void initComplete(Surface glView);

	
    public static void loadLibrary(String name) {
		Log.d("Unity", "loading library " + name);
        java.lang.System.loadLibrary(name);
    }
}