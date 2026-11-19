using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Temporary IMGUI screens used to connect the early story-flow scenes.
/// Replace the placeholder panels with the final UI and story content later.
/// </summary>
public sealed class StoryFlowController : MonoBehaviour
{
    private const string TitleScene = "Title";
    private const string PrologueScene = "Prologue";
    private const string DayScene = "Day";
    private const string InterviewScene = "Interview";
    private const string SmartphoneScene = "Smartphone";
    private const string ChapterEndingScene = "ChapterEnding";
    private const string TrueEndingScene = "TrueEnding";
    private const string GameOverScene = "GameOver";

    private GUIStyle titleStyle;
    private GUIStyle sectionStyle;
    private GUIStyle bodyStyle;
    private GUIStyle smallStyle;
    private GUIStyle buttonStyle;
    private GUIStyle selectedButtonStyle;
    private string interviewNotice;
    private string phoneNotice;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == SmartphoneScene)
        {
            StoryFlowProgress.AdvancePhoneClock(Time.unscaledDeltaTime);
        }
    }

    private void OnGUI()
    {
        EnsureStyles();

        Color previousColor = GUI.color;
        GUI.color = new Color(0.08f, 0.11f, 0.16f, 1f);
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);

        float panelWidth = Mathf.Min(900f, Mathf.Max(300f, Screen.width - 32f));
        float panelHeight = Mathf.Min(720f, Mathf.Max(300f, Screen.height - 32f));
        Rect panel = new Rect(
            (Screen.width - panelWidth) * 0.5f,
            (Screen.height - panelHeight) * 0.5f,
            panelWidth,
            panelHeight
        );

        GUI.color = new Color(0.97f, 0.97f, 0.94f, 1f);
        GUI.DrawTexture(panel, Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUILayout.BeginArea(new Rect(
            panel.x + 36f,
            panel.y + 28f,
            panel.width - 72f,
            panel.height - 56f
        ));

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == TitleScene)
        {
            DrawTitle();
        }
        else if (sceneName == PrologueScene)
        {
            DrawPrologue();
        }
        else if (sceneName == DayScene)
        {
            DrawDay();
        }
        else if (sceneName == InterviewScene)
        {
            DrawInterview();
        }
        else if (sceneName == SmartphoneScene)
        {
            DrawSmartphone();
        }
        else if (sceneName == ChapterEndingScene)
        {
            DrawChapterEnding();
        }
        else if (sceneName == TrueEndingScene)
        {
            DrawTrueEnding();
        }
        else if (sceneName == GameOverScene)
        {
            DrawGameOver();
        }
        else
        {
            GUILayout.Label("Story flow scene", titleStyle);
            GUILayout.Label("この画面の構成は準備中です。", bodyStyle);
            if (GUILayout.Button("タイトルへ戻る", buttonStyle, GUILayout.Height(48f)))
            {
                LoadScene(TitleScene);
            }
        }

        GUILayout.EndArea();
        GUI.color = previousColor;
    }

    private void EnsureStyles()
    {
        if (titleStyle != null)
        {
            return;
        }

        titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 34,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
        titleStyle.normal.textColor = new Color(0.10f, 0.16f, 0.25f, 1f);

        sectionStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 23,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
            wordWrap = true
        };
        sectionStyle.normal.textColor = new Color(0.16f, 0.24f, 0.34f, 1f);

        bodyStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 19,
            alignment = TextAnchor.MiddleLeft,
            wordWrap = true,
            richText = true
        };
        bodyStyle.normal.textColor = new Color(0.20f, 0.23f, 0.27f, 1f);

        smallStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 15,
            alignment = TextAnchor.MiddleLeft,
            wordWrap = true
        };
        smallStyle.normal.textColor = new Color(0.35f, 0.39f, 0.43f, 1f);

        buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 17,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
        buttonStyle.normal.textColor = new Color(0.12f, 0.18f, 0.25f, 1f);

        selectedButtonStyle = new GUIStyle(buttonStyle);
        selectedButtonStyle.normal.textColor = new Color(0.04f, 0.40f, 0.48f, 1f);
        selectedButtonStyle.fontStyle = FontStyle.Bold;
    }

    private void DrawTitle()
    {
        GUILayout.Label("タイトル画面（仮）", titleStyle);
        GUILayout.Space(12f);
        GUILayout.Label(
            "三つの時代に残る投稿の痕跡を追い、匿名アカウントのつながりを調べます。",
            bodyStyle
        );
        GUILayout.Space(24f);
        GUILayout.Label("難易度を選択", sectionStyle);
        GUILayout.Space(8f);

        GUILayout.BeginHorizontal();
        DrawDifficultyOption(StoryFlowProgress.Difficulty.Easy, "EASY");
        DrawDifficultyOption(StoryFlowProgress.Difficulty.Normal, "NORMAL");
        DrawDifficultyOption(StoryFlowProgress.Difficulty.Hard, "HARD");
        GUILayout.EndHorizontal();

        GUILayout.Space(8f);
        GUILayout.Label("難易度ごとの細かなルールは今後設定します。", smallStyle);
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("ゲームを始める", buttonStyle, GUILayout.Height(56f)))
        {
            StoryFlowProgress.StartNewGame();
            LoadScene(PrologueScene);
        }

        GUILayout.Space(12f);
        GUILayout.Label("画面・会話・調査条件はプロトタイプ用の仮置きです。", smallStyle);
    }

    private void DrawDifficultyOption(StoryFlowProgress.Difficulty difficulty, string label)
    {
        bool selected = StoryFlowProgress.SelectedDifficulty == difficulty;
        GUIStyle style = selected ? selectedButtonStyle : buttonStyle;
        if (GUILayout.Toggle(selected, label, style, GUILayout.Height(50f)))
        {
            StoryFlowProgress.SetDifficulty(difficulty);
        }
    }

    private void DrawPrologue()
    {
        GUILayout.Label("CHAPTER " + (StoryFlowProgress.CurrentChapter + 1).ToString("00"), smallStyle);
        GUILayout.Space(8f);
        GUILayout.Label(StoryFlowProgress.GetChapterTitle(), titleStyle);
        GUILayout.Space(18f);
        GUILayout.Label(StoryFlowProgress.GetChapterDescription(), bodyStyle);
        GUILayout.Space(24f);
        GUILayout.Label("調査期限の目安：各編最大7日", sectionStyle);
        GUILayout.Label("その編で見つけた核心記録を、次の時代へ持ち越す想定です。", bodyStyle);
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("調査を始める", buttonStyle, GUILayout.Height(56f)))
        {
            LoadScene(DayScene);
        }
    }

    private void DrawDay()
    {
        GUILayout.Label(StoryFlowProgress.GetChapterTitle(), sectionStyle);
        GUILayout.Label(
            "調査 " + StoryFlowProgress.CurrentDay + " / " + StoryFlowProgress.DaysPerChapter + " 日目",
            titleStyle
        );
        GUILayout.Space(8f);
        GUILayout.Label(
            StoryFlowProgress.GetPhaseName() + "　" + StoryFlowProgress.GetCurrentTimeLabel(),
            sectionStyle
        );
        GUILayout.Label(StoryFlowProgress.GetScheduleSummary(), smallStyle);
        GUILayout.Space(18f);
        GUILayout.Label("この時間帯の行動", sectionStyle);
        GUILayout.Space(8f);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("聞き込み（仮）", buttonStyle, GUILayout.Height(52f)))
        {
            interviewNotice = null;
            LoadScene(InterviewScene);
        }
        if (GUILayout.Button("スマホで調べる（仮）", buttonStyle, GUILayout.Height(52f)))
        {
            phoneNotice = null;
            LoadScene(SmartphoneScene);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(12f);
        if (!string.IsNullOrEmpty(interviewNotice))
        {
            GUILayout.Label(interviewNotice, smallStyle);
        }

        GUILayout.FlexibleSpace();
        string advanceLabel = StoryFlowProgress.CurrentPhase < 2
            ? "次の時間帯へ"
            : "一日を終える";

        if (GUILayout.Button(advanceLabel, buttonStyle, GUILayout.Height(54f)))
        {
            bool chapterFinished = StoryFlowProgress.AdvanceTimeSlotOrDay();
            if (chapterFinished)
            {
                if (StoryFlowProgress.CurrentChapter == 2 &&
                    StoryFlowProgress.HasAllCoreEvidence)
                {
                    LoadScene(TrueEndingScene);
                }
                else
                {
                    LoadScene(ChapterEndingScene);
                }
            }
        }
    }

    private void DrawInterview()
    {
        GUILayout.Label("聞き込み（仮）", titleStyle);
        GUILayout.Space(14f);
        GUILayout.Label(
            StoryFlowProgress.GetChapterTitle() + " / " +
            StoryFlowProgress.GetPhaseName() + " / " +
            StoryFlowProgress.GetCurrentTimeLabel(),
            sectionStyle
        );
        GUILayout.Space(14f);
        GUILayout.Label(
            "登場人物、会話文、選択肢、調査結果を置くための場面です。",
            bodyStyle
        );
        GUILayout.Label(
            "会話内容と手がかりの条件は、企画が固まった段階で接続します。",
            smallStyle
        );
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("行動選択へ戻る", buttonStyle, GUILayout.Height(54f)))
        {
            LoadScene(DayScene);
        }
    }

    private void DrawSmartphone()
    {
        GUILayout.Label("スマートフォン（仮）", titleStyle);
        GUILayout.Space(8f);
        GUILayout.Label(
            StoryFlowProgress.GetChapterTitle() + " / " +
            StoryFlowProgress.GetPhaseName() + " / " +
            StoryFlowProgress.GetCurrentTimeLabel(),
            sectionStyle
        );
        GUILayout.Label("閲覧中はリアル5秒ごとにゲーム内時間が1分進みます。", smallStyle);
        GUILayout.Space(18f);

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("投稿一覧（仮）", sectionStyle);
        GUILayout.Label("同じ短い言い回しを含む投稿。本文やアカウント情報は準備中です。", bodyStyle);
        GUILayout.EndVertical();

        GUILayout.Space(14f);
        if (StoryFlowProgress.HasCurrentCoreEvidence)
        {
            GUILayout.Label("この編の核心記録：仮登録済み", sectionStyle);
        }
        else
        {
            if (GUILayout.Button("この投稿を核心記録として保存（仮）", buttonStyle, GUILayout.Height(52f)))
            {
                StoryFlowProgress.MarkCurrentCoreEvidence();
                phoneNotice = "この編の記録を仮登録しました。";
            }
        }

        if (!string.IsNullOrEmpty(phoneNotice))
        {
            GUILayout.Label(phoneNotice, smallStyle);
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("行動選択へ戻る", buttonStyle, GUILayout.Height(54f)))
        {
            LoadScene(DayScene);
        }
    }

    private void DrawChapterEnding()
    {
        bool isFinalChapter = StoryFlowProgress.CurrentChapter == 2;
        GUILayout.Label(
            isFinalChapter ? "各編エンド（仮）" : "章の区切り（仮）",
            titleStyle
        );
        GUILayout.Space(16f);
        GUILayout.Label(StoryFlowProgress.GetChapterTitle(), sectionStyle);

        if (isFinalChapter)
        {
            GUILayout.Label(
                "三編の核心記録がそろっていないため、連鎖の真相には届かない結末です。",
                bodyStyle
            );
            GUILayout.Label("最終条件と本文は今後調整します。", smallStyle);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("タイトルへ戻る", buttonStyle, GUILayout.Height(54f)))
            {
                LoadScene(TitleScene);
            }
            return;
        }

        GUILayout.Label(
            "この編の事件に区切りをつけ、見つけた記録を次の時代へ引き継ぎます。",
            bodyStyle
        );
        GUILayout.Label("章末の会話と記録の受け渡しは準備中です。", smallStyle);
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("次の時代へ進む", buttonStyle, GUILayout.Height(54f)))
        {
            StoryFlowProgress.BeginNextChapter();
            LoadScene(PrologueScene);
        }
    }

    private void DrawTrueEnding()
    {
        GUILayout.Label("真エンド（仮）", titleStyle);
        GUILayout.Space(16f);
        GUILayout.Label(
            "中学・高校・大学の三編で集めた核心記録をつなぎ、匿名アカウント網の源流を特定します。",
            bodyStyle
        );
        GUILayout.Label("結末の内容はストーリー本文の確定後に追加します。", smallStyle);
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("タイトルへ戻る", buttonStyle, GUILayout.Height(54f)))
        {
            LoadScene(TitleScene);
        }
    }

    private void DrawGameOver()
    {
        GUILayout.Label("ゲームオーバー", titleStyle);
        GUILayout.Space(16f);
        GUILayout.Label(
            "主人公のアカウントが削除され、調査を続けられなくなりました。",
            bodyStyle
        );
        GUILayout.Label("この場面へつなぐ削除イベントは今後実装します。", smallStyle);
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("タイトルへ戻る", buttonStyle, GUILayout.Height(54f)))
        {
            LoadScene(TitleScene);
        }
    }

    /// <summary>
    /// Call this from the future account-deletion event when that condition is implemented.
    /// </summary>
    public void TriggerAccountDeletionGameOver()
    {
        LoadScene(GameOverScene);
    }

    private static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

internal static class StoryFlowProgress
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public const int DaysPerChapter = 7;

    private static readonly string[] ChapterTitles =
    {
        "中学編｜最初の痕跡",
        "高校編｜つながる噂",
        "大学編｜源流の特定"
    };

    private static readonly int[,] PhaseStartMinutes =
    {
        { 8 * 60 + 20, 12 * 60 + 35, 15 * 60 + 15 },
        { 8 * 60 + 30, 12 * 60 + 35, 15 * 60 + 15 },
        { 9 * 60, 12 * 60 + 10, 18 * 60 }
    };

    private static readonly string[,] PhaseNames =
    {
        { "朝礼", "昼休み", "放課後" },
        { "朝礼", "昼休み", "放課後" },
        { "授業開始", "昼休み", "放課後" }
    };

    private static readonly int[] InterviewCounts = new int[3];

    private static int chapter;
    private static int day = 1;
    private static int phase;
    private static int coreEvidenceMask;
    private static int phoneMinutes;
    private static float phoneSecondsRemainder;
    private static Difficulty selectedDifficulty = Difficulty.Normal;

    public static int CurrentChapter => chapter;
    public static int CurrentDay => day;
    public static int CurrentPhase => phase;
    public static Difficulty SelectedDifficulty => selectedDifficulty;
    public static bool HasAllCoreEvidence => (coreEvidenceMask & 7) == 7;
    public static bool HasCurrentCoreEvidence => (coreEvidenceMask & (1 << chapter)) != 0;

    public static void SetDifficulty(Difficulty difficulty)
    {
        selectedDifficulty = difficulty;
    }

    public static void StartNewGame()
    {
        chapter = 0;
        day = 1;
        phase = 0;
        coreEvidenceMask = 0;
        phoneMinutes = 0;
        phoneSecondsRemainder = 0f;
        for (int i = 0; i < InterviewCounts.Length; i++)
        {
            InterviewCounts[i] = 0;
        }
    }

    public static string GetChapterTitle()
    {
        return ChapterTitles[Mathf.Clamp(chapter, 0, ChapterTitles.Length - 1)];
    }

    public static string GetChapterDescription()
    {
        switch (chapter)
        {
            case 0:
                return "教室に広がった匿名の噂をたどり、投稿の断片と聞き込みから発信者を追います。";
            case 1:
                return "同じ短文を含む噂が再燃。複数のアカウントと、前の編で欠けた記録を調べます。";
            default:
                return "大学内で連鎖して消える告発投稿を照合し、最初の投稿者や拡散経路を追います。";
        }
    }

    public static string GetPhaseName()
    {
        return PhaseNames[Mathf.Clamp(chapter, 0, 2), Mathf.Clamp(phase, 0, 2)];
    }

    public static string GetScheduleSummary()
    {
        if (chapter == 0)
        {
            return "時間の目安：8:20 朝礼／12:35–13:15 昼休み／15:15〜放課後";
        }
        if (chapter == 1)
        {
            return "時間の目安：8:30 朝礼／12:35–13:15 昼休み／15:15〜放課後";
        }
        return "時間の目安：9:00 授業開始／12:10–13:00 昼休み／18:00〜放課後";
    }

    public static string GetCurrentTimeLabel()
    {
        int totalMinutes =
            PhaseStartMinutes[Mathf.Clamp(chapter, 0, 2), Mathf.Clamp(phase, 0, 2)] +
            phoneMinutes;
        return (totalMinutes / 60).ToString("00") + ":" +
               (totalMinutes % 60).ToString("00");
    }

    public static void RecordInterview()
    {
        InterviewCounts[Mathf.Clamp(chapter, 0, InterviewCounts.Length - 1)]++;
    }

    public static void MarkCurrentCoreEvidence()
    {
        coreEvidenceMask |= 1 << chapter;
    }

    public static void AdvancePhoneClock(float elapsedSeconds)
    {
        phoneSecondsRemainder += Mathf.Max(0f, elapsedSeconds);
        while (phoneSecondsRemainder >= 5f)
        {
            phoneSecondsRemainder -= 5f;
            phoneMinutes++;
        }
    }

    /// <returns>True when the current chapter's seventh day has ended.</returns>
    public static bool AdvanceTimeSlotOrDay()
    {
        phoneMinutes = 0;
        phoneSecondsRemainder = 0f;

        if (phase < 2)
        {
            phase++;
            return false;
        }

        if (day < DaysPerChapter)
        {
            day++;
            phase = 0;
            return false;
        }

        return true;
    }

    public static void BeginNextChapter()
    {
        chapter = Mathf.Clamp(chapter + 1, 0, 2);
        day = 1;
        phase = 0;
        phoneMinutes = 0;
        phoneSecondsRemainder = 0f;
    }
}
